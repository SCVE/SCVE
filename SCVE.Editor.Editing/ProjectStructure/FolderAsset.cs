using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure;

public class FolderAsset : Asset<Folder>
{
    [JsonConstructor]
    private FolderAsset()
    {
    }

    public static FolderAsset CreateNew(string name, string location, Folder content)
    {
        return new FolderAsset()
        {
            Guid = Guid.NewGuid(),
            Name = name,
            Location = location,
            Content = content
        };
    }

    public override void AcceptVisitor(IAssetVisitor visitor)
    {
        visitor.Visit(this);
    }
}