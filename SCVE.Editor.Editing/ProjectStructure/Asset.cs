namespace SCVE.Editor.Editing.ProjectStructure
{
    public abstract class Asset<T> : AssetBase
    {
        public T Content { get; set; }

        public Asset()
        {
        }
    }
}