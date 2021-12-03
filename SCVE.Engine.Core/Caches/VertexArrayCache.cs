using System.Collections.Generic;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.Core.Caches
{
    public class VertexArrayCache
    {
        private Dictionary<string, VertexArray> _vertexArrays = new();

        public VertexArray Get(string name)
        {
            return _vertexArrays.ContainsKey(name) ? _vertexArrays[name] : null;
        }

        public void AddOrReplace(string name, VertexArray vertexArray)
        {
            if (_vertexArrays.ContainsKey(name))
            {
                var array = _vertexArrays[name];
                array.Dispose();
                _vertexArrays.Remove(name);
            }
            _vertexArrays.Add(name, vertexArray);
        }
    }
}