using System;

namespace SCVE.Editor.ProjectStructure
{
    public class ProjectAssetFileData
    {
        public Guid Guid { get; set; }
        public string Type { get; set; }

        public string FileSystemPath { get; set; }

        public ProjectAssetFileData()
        {
        }

        public ProjectAssetFileData(Guid guid, string type, string fileSystemPath)
        {
            Guid           = guid;
            Type           = type;
            FileSystemPath = fileSystemPath;
        }
    }
}