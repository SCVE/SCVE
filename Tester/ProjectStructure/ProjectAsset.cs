namespace Tester.ProjectStructure
{
    public abstract class ProjectAsset<T> : ProjectAssetBase
    {
        public T Asset { get; set; }

        private Lazy<T> _lazyAsset;

        public ProjectAsset()
        {
        }

        public ProjectAsset(Func<string, string, T> factory)
        {
            // TODO: think about it
            _lazyAsset = new Lazy<T>(() => factory(Location, Name));
        }
    }
}