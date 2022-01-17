using System;
using System.IO;

namespace SCVE.Editor.ProjectStructure
{
    public class ProjectAsset
    {
        public Guid Guid { get; set; }
        
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

        private ProjectAsset()
        {
            Guid = new Guid();
        }

        public ProjectAsset(Guid guid, string internalName, string internalFullPath, string fileSystemFullPath, string type)
        {
            Guid               = guid;
            InternalName       = internalName;
            InternalFullPath   = internalFullPath;
            FileSystemFullPath = fileSystemFullPath;
            Type               = type;
        }
    }
}