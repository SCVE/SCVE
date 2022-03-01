using System;

namespace SCVE.Editor.ProjectStructure
{
    public class ProjectAssetBase
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }
    }
    
    public abstract class ProjectAsset<T> : ProjectAssetBase
    {
        public T Asset { get; set; }

        private Lazy<T> _lazyAsset;

        public ProjectAsset(Func<string, string, T> factory)
        {
            // TODO: think about it
            _lazyAsset = new Lazy<T>(() => factory(Location, Name));
        }
    }
}