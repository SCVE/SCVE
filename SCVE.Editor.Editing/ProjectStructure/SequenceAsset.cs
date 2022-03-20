using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class SequenceAsset : Asset<Sequence>
    {
        [JsonConstructor]
        private SequenceAsset()
        {
        }

        public static SequenceAsset CreateNew(string name, string location, Sequence content)
        {
            return new SequenceAsset()
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
}