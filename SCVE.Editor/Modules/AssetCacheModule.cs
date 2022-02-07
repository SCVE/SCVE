using SCVE.Editor.Caching;

namespace SCVE.Editor.Modules;

public class AssetCacheModule : IModule
{
    public AssetCache Cache { get; set; }

    public void OnUpdate()
    {
    }

    public void OnInit()
    {
        Cache = new AssetCache();
    }

    public void CrossReference(ModulesContainer modulesContainer)
    {
    }
}