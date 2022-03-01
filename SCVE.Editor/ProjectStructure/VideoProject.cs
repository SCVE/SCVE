using System.Collections.Generic;
using SCVE.Editor.Editing;

namespace SCVE.Editor.ProjectStructure
{
    public class VideoProject
    {
        public ICollection<ProjectAsset<Sequence>> Sequences { get; set; }
        public ICollection<ProjectAsset<Image>> Images { get; set; }

        public VideoProject()
        {
        }
    }
}