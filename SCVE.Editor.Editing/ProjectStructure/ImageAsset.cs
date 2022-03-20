using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class ImageAsset : Asset<Image>
    {
        [JsonConstructor]
        private ImageAsset()
        {
        }

        public static ImageAsset CreateNew(string name, string location, Image content)
        {
            return new ImageAsset()
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