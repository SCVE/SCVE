using System;

namespace SCVE.Editor.ProjectStructure
{
    public class ProjectImageAsset : ProjectAsset<Image>
    {
        public ProjectImageAsset(Func<string, string, Image> factory) : base(factory)
        {
        }
    }
}