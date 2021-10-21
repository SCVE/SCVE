using System.Collections.Generic;
using SCVE.Core.Rendering;

namespace SCVE.Core.Services
{
    public class VertexArrayCache
    {
        private VertexArray _default;
        private Dictionary<string, VertexArray> _vertexArrays = new();

        public VertexArray Get(string name)
        {
            return _vertexArrays.ContainsKey(name) ? _vertexArrays[name] : _default;
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

        public void SetDefault(VertexArray vertexArray)
        {
            _default = vertexArray;
        }
    }
}