using SCVE.Editor.Caching;

namespace SCVE.Editor.Modules
{
    public class AssetCacheService : IService
    {
        public AssetCache Cache { get; set; }

        public AssetCacheService()
        {
            Cache = new AssetCache();
        }

        public void OnUpdate()
        {
        }
    }
}