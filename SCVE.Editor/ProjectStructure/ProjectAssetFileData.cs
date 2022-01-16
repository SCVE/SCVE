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

        public ProjectAssetFileData(string type, string fileSystemPath)
        {
            Type           = type;
            FileSystemPath = fileSystemPath;
        }
    }
}