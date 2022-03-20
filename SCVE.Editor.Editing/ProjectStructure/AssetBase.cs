namespace SCVE.Editor.Editing.ProjectStructure
{
    public abstract class AssetBase
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }
}