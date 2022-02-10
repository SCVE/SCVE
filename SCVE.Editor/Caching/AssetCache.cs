using System.Collections.Generic;
using System.IO;

namespace SCVE.Editor.Caching
{

    public class AssetCache
    {
        private Dictionary<string, byte[]> _assets;

        public AssetCache()
        {
            _assets = new Dictionary<string, byte[]>();
        }

        public byte[] GetOrCache(string path)
        {
            if (_assets.ContainsKey(path))
            {
                return _assets[path];
            }
            else
            {
                var bytes = File.ReadAllBytes(path);
                _assets[path] = bytes;
                return bytes;
            }
        }
    }
}