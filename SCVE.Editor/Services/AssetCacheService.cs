using System.Collections.Generic;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Caching;
using Silk.NET.Input;

namespace SCVE.Editor.Services
{
    public class AssetCacheService : IService
    {
        public AssetCache Cache { get; set; }

        public AssetCacheService()
        {
            Cache = new AssetCache();
        }
    }
}