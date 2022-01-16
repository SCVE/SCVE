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

        public ProjectAssetFolder(string internalName, string internalFullPath) : base(internalName, internalFullPath, "", "FOLDER")
        {
            _assets     = new();
            _subfolders = new();
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
            _subfolders.Add(new ProjectAssetFolder(name, projectFullPath));
        }

        public void AppendEmptyFolder(string name, string internalFullPath)
        {
            _subfolders.Add(new ProjectAssetFolder(name, internalFullPath));
        }

        private void AppendRawAsset(string name, string internalFullPath, string fileSystemFullPath, string type)
        {
            _assets.Add(new ProjectAsset(name, internalFullPath, fileSystemFullPath, type));
        }

        public void AppendAsset(string internalRelativePath, string internalName, string internalFullPath, string fileSystemFullPath, string type)
        {
            if (internalRelativePath == Path.DirectorySeparatorChar.ToString())
            {
                AppendRawAsset(internalName, internalFullPath, fileSystemFullPath, type);
            }
            else if (!internalRelativePath.Contains(Path.DirectorySeparatorChar))
            {
                string folderName = internalRelativePath;
                if (folderName == "")
                {
                    // if name is empty - we are adding an empty folder
                    AppendRawAsset(internalName, internalFullPath, fileSystemFullPath, type);
                }
                else
                {
                    if (!HasDirectChildFolder(folderName))
                    {
                        AppendFolder(folderName, InternalFullPath + internalRelativePath);
                    }

                    GetDirectChildFolder(folderName).AppendRawAsset(internalName, internalFullPath, fileSystemFullPath, type);
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

                GetDirectChildFolder(folderName).AppendAsset(nextPath, internalName, internalFullPath, fileSystemFullPath, type);
            }
        }
    }
}