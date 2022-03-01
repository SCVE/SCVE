using Tester.Editing;

namespace Tester.ProjectStructure
{
    public class ProjectSequenceAsset : ProjectAsset<Sequence>
    {
        public ProjectSequenceAsset()
        {
        }

        public ProjectSequenceAsset(Func<string, string, Sequence> factory) : base(factory)
        {
        }
    }
}