using System.IO.Compression;

namespace SCVE.Editor
{
    public class ProjectAsset
    {
        public virtual string Name { get; }

        public ProjectAsset(string name)
        {
            Name = name;
        }
    }
}