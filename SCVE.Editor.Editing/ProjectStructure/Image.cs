using System.Globalization;
using System.Text.Json.Serialization;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class Image
    {
        public Guid Guid { get; set; }

        public string RelativePath { get; set; }
        
        [JsonConstructor]
        private Image()
        {
        }

        public static Image CreateNew(string relativePath)
        {
            return new Image()
            {
                Guid = Guid.NewGuid(),
                RelativePath = relativePath
            };
        }
    }
}