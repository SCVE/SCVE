using Tester.Editing;

namespace Tester.ProjectStructure
{
    public class VideoProject
    {
        public ICollection<ProjectSequenceAsset> Sequences { get; set; }
        public ICollection<ProjectImageAsset> Images { get; set; }

        public VideoProject()
        {
        }

        private Sequence LoadSequence(string location, string name)
        {
            return null;
        }

        private Image LoadImage()
        {
            return null;
        }
    }
}