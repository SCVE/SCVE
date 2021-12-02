using System.Collections.Generic;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.Core.Caches
{
    public class VertexCacheInitiator
    {
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

        public void Init(VertexArrayCache vertexArrayCache)
        {
            foreach (var keyValuePair in _vertexArrays)
            {
                vertexArrayCache.AddOrReplace(keyValuePair.Key, keyValuePair.Value);
            }
        }
    }
}