namespace SCVE.Editor
{
    public class ProjectMetaFileData
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string Version { get; set; }

        public ProjectMetaFileData()
        {
        }

        public ProjectMetaFileData(string name, string type, string version)
        {
            Type      = type;
            Version   = version;
            Name = name;
        }
    }
}