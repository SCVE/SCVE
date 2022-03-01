namespace Tester.ProjectStructure
{
    public class ProjectImageAsset : ProjectAsset<Image>
    {
        public ProjectImageAsset()
        {
        }

        public ProjectImageAsset(Func<string, string, Image> factory) : base(factory)
        {
        }
    }
}