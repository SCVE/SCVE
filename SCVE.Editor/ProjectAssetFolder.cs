using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SCVE.Editor
{
    public class ProjectAssetFolder : ProjectAsset
    {
        public IReadOnlyList<ProjectAsset> Assets => _assets;
        private readonly List<ProjectAsset> _assets;

        public IReadOnlyList<ProjectAssetFolder> Subfolders => _subfolders;
        private readonly List<ProjectAssetFolder> _subfolders;

        public ProjectAssetFolder(string name) : base(name)
        {
            _assets     = new();
            _subfolders = new();
        }

        private bool HasFolder(string name)
        {
            return _subfolders.Any(f => f.Name == name);
        }

        private ProjectAssetFolder GetFolder(string name)
        {
            return _subfolders.First(f => f.Name == name);
        }

        private void AppendFolder(string name)
        {
            _subfolders.Add(new ProjectAssetFolder(name));
        }

        public void AppendEmptyFolder(string name)
        {
            _subfolders.Add(new ProjectAssetFolder(name));
        }

        private void AppendRawAsset(string name)
        {
            _assets.Add(new ProjectAsset(name));
        }

        /// <summary>
        /// Path must end with /
        /// </summary>
        public void AppendAsset(string path, string name)
        {
            if (path == Path.DirectorySeparatorChar.ToString())
            {
                AppendRawAsset(name);
            }
            else if (!path.Contains(Path.DirectorySeparatorChar))
            {
                if (path == "")
                {
                    AppendRawAsset(name);
                    return;
                }
                if (!HasFolder(path))
                {
                    AppendFolder(path);
                }
                GetFolder(path).AppendRawAsset(name);
            }
            else
            {
                string nextPath = path.Substring(path.IndexOf(Path.DirectorySeparatorChar) + 1);
                string folder   = path.Substring(0, path.IndexOf(Path.DirectorySeparatorChar));
                if (!HasFolder(folder))
                {
                    AppendFolder(folder);
                }

                GetFolder(folder).AppendAsset(nextPath, name);
            }
        }
    }
}