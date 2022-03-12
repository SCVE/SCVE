using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class SequenceAsset : Asset<Sequence>
    {
        public SequenceAsset()
        {
        }

        public override void AcceptVisitor(IAssetVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}