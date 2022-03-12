using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure;

public class FolderAsset : Asset<Folder>
{
    public override void AcceptVisitor(IAssetVisitor visitor)
    {
        visitor.Visit(this);
    }
}