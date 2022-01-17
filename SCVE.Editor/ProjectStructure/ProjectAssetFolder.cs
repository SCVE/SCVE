using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SCVE.Editor.ProjectStructure
{
    public class ProjectAssetFolder : ProjectAsset
    {
        public IReadOnlyList<ProjectAsset> Assets => _assets;
        private readonly List<ProjectAsset> _assets;

        public IReadOnlyList<ProjectAssetFolder> Subfolders => _subfolders;
        private readonly List<ProjectAssetFolder> _subfolders;

        public ProjectAssetFolder(Guid guid, string internalName, string internalFullPath) : base(guid, internalName, internalFullPath, "", "FOLDER")
        {
            _assets     = new();
            _subfolders = new();
        }

        public T FindAsset<T>(Guid guid) where T : ProjectAsset
        {
            var directAsset = _assets.FirstOrDefault(a => a.Guid == guid);
            if (directAsset is not null)
            {
                return directAsset as T;
            }
            else
            {
                foreach (var subfolder in _subfolders)
                {
                    var subfolderAsset = subfolder.FindAsset<T>(guid);
                    if (subfolderAsset is not null)
                    {
                        return subfolderAsset;
                    }
                }

                return null;
            }
        }

        private bool HasDirectChildFolder(string name)
        {
            return _subfolders.Any(f => f.InternalName == name);
        }

        public ProjectAssetFolder GetDirectChildFolder(string name)
        {
            return _subfolders.First(f => f.InternalName == name);
        }

        private void AppendFolder(string name, string projectFullPath)
        {
            _subfolders.Add(new ProjectAssetFolder(Guid.Empty, name, projectFullPath));
        }

        public void AppendEmptyFolder(string name, string internalFullPath)
        {
            _subfolders.Add(new ProjectAssetFolder(Guid.Empty, name, internalFullPath));
        }

        private void AppendRawAsset(ProjectAsset asset)
        {
            _assets.Add(asset);
        }

        public void AppendAsset(string internalRelativePath, ProjectAsset asset)
        {
            if (internalRelativePath == Path.DirectorySeparatorChar.ToString())
            {
                AppendRawAsset(asset);
            }
            else if (!internalRelativePath.Contains(Path.DirectorySeparatorChar))
            {
                string folderName = internalRelativePath;
                if (folderName == "")
                {
                    // if name is empty - we are adding an empty folder
                    AppendRawAsset(asset);
                }
                else
                {
                    if (!HasDirectChildFolder(folderName))
                    {
                        AppendFolder(folderName, InternalFullPath + internalRelativePath);
                    }

                    GetDirectChildFolder(folderName).AppendRawAsset(asset);
                }
            }
            else
            {
                string nextPath = internalRelativePath.Substring(internalRelativePath.IndexOf(Path.DirectorySeparatorChar) + 1);
                string folderName   = internalRelativePath.Substring(0, internalRelativePath.IndexOf(Path.DirectorySeparatorChar));
                if (!HasDirectChildFolder(folderName))
                {
                    AppendFolder(folderName, InternalFullPath + folderName);
                }

                GetDirectChildFolder(folderName).AppendAsset(nextPath, asset);
            }
        }
    }
}