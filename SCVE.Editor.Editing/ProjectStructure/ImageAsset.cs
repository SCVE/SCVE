using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class ImageAsset : Asset<Image>
    {
        public override void AcceptVisitor(IAssetVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}