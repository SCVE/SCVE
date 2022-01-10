using System.IO;

namespace SCVE.Editor
{
    public class ProjectAsset
    {
        /// <summary>
        /// Name of an asset, local to this project
        /// </summary>
        public string InternalName { get; }

        /// <summary>
        /// Full path of an asset, relative to the project root
        /// </summary>
        public string InternalFullPath { get; }

        /// <summary>
        /// Full path of an asset in a file system
        /// </summary>
        public string FileSystemFullPath { get; }

        public string Type { get; }

        public bool ExistsInFileSystem => File.Exists(FileSystemFullPath);

        public ProjectAsset(string internalName, string internalFullPath, string fileSystemFullPath, string type)
        {
            InternalName       = internalName;
            InternalFullPath   = internalFullPath;
            FileSystemFullPath = fileSystemFullPath;
            Type               = type;
        }
    }
}