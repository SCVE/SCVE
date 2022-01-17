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

        public ProjectAsset FindAsset(Guid guid)
        {
            var asset = _assets.FirstOrDefault(a => a.Guid == guid);
            if (asset is not null)
            {
                return asset;
            }
            else
            {
                foreach (var subfolder in _subfolders)
                {
                    asset = subfolder.FindAsset(guid);
                    if (asset is not null)
                    {
                        return asset;
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

        private void AppendFolder(Guid guid, string name, string projectFullPath)
        {
            _subfolders.Add(new ProjectAssetFolder(guid, name, projectFullPath));
        }

        public void AppendEmptyFolder(Guid guid, string name, string internalFullPath)
        {
            _subfolders.Add(new ProjectAssetFolder(guid, name, internalFullPath));
        }

        private void AppendRawAsset(Guid guid, string name, string internalFullPath, string fileSystemFullPath, string type)
        {
            _assets.Add(new ProjectAsset(guid, name, internalFullPath, fileSystemFullPath, type));
        }

        public void AppendAsset(Guid guid, string internalRelativePath, string internalName, string internalFullPath, string fileSystemFullPath, string type)
        {
            if (internalRelativePath == Path.DirectorySeparatorChar.ToString())
            {
                AppendRawAsset(guid, internalName, internalFullPath, fileSystemFullPath, type);
            }
            else if (!internalRelativePath.Contains(Path.DirectorySeparatorChar))
            {
                string folderName = internalRelativePath;
                if (folderName == "")
                {
                    // if name is empty - we are adding an empty folder
                    AppendRawAsset(guid, internalName, internalFullPath, fileSystemFullPath, type);
                }
                else
                {
                    if (!HasDirectChildFolder(folderName))
                    {
                        AppendFolder(guid, folderName, InternalFullPath + internalRelativePath);
                    }

                    GetDirectChildFolder(folderName).AppendRawAsset(guid, internalName, internalFullPath, fileSystemFullPath, type);
                }
            }
            else
            {
                string nextPath = internalRelativePath.Substring(internalRelativePath.IndexOf(Path.DirectorySeparatorChar) + 1);
                string folderName   = internalRelativePath.Substring(0, internalRelativePath.IndexOf(Path.DirectorySeparatorChar));
                if (!HasDirectChildFolder(folderName))
                {
                    AppendFolder(guid, folderName, InternalFullPath + folderName);
                }

                GetDirectChildFolder(folderName).AppendAsset(guid, nextPath, internalName, internalFullPath, fileSystemFullPath, type);
            }
        }
    }
}