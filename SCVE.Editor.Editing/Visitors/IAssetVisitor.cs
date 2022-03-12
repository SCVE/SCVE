using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Editing.Visitors;

public interface IAssetVisitor
{
    void Visit(ImageAsset asset);
    void Visit(SequenceAsset asset);
    void Visit(FolderAsset asset);
}