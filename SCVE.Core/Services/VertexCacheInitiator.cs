using System.Collections.Generic;
using SCVE.Core.Rendering;

namespace SCVE.Core.Services
{
    public class VertexCacheInitiator
    {
        private VertexArray _default;
        private Dictionary<string, VertexArray> _vertexArrays = new();

        public VertexCacheInitiator()
        {
            _vertexArrays = new();
        }

        public VertexCacheInitiator With(string name, VertexArray vertexArray)
        {
            _vertexArrays.Add(name, vertexArray);
            return this;
        }

        public VertexCacheInitiator WithDefault(VertexArray vertexArray)
        {
            _default = vertexArray;
            return this;
        }

        public void Init(VertexArrayCache vertexArrayCache)
        {
            foreach (var keyValuePair in _vertexArrays)
            {
                vertexArrayCache.AddOrReplace(keyValuePair.Key, keyValuePair.Value);
            }
            vertexArrayCache.SetDefault(_default);
        }
    }
}