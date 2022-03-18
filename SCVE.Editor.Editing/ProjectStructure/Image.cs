using System.Text.Json.Serialization;

namespace SCVE.Editor.Editing.ProjectStructure
{
    public class Image
    {
        public Guid Guid { get; set; }

        public string RelativePath { get; set; }
        
        public Image()
        {
               
        }
    }
}