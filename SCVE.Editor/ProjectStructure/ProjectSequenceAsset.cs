using System;
using SCVE.Editor.Editing;

namespace SCVE.Editor.ProjectStructure
{
    public class ProjectSequenceAsset : ProjectAsset<Sequence>
    {
        public ProjectSequenceAsset(Func<string, string, Sequence> factory) : base(factory)
        {
        }
    }
}