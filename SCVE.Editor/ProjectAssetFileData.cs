namespace SCVE.Editor
{
    public class ProjectAssetFileData
    {
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