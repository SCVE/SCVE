using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public abstract class Asset<T> : AssetBase
    {
        public T Content { get; set; }

        public Asset()
        {
        }

        public abstract void AcceptVisitor(IAssetVisitor visitor);
    }
}