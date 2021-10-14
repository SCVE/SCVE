using System.Collections.Generic;

namespace SCVE.Core.Rendering
{
    public class VertexBufferLayout
    {
        private List<VertexBufferElement> _elements;

        private int _stride;

        public IReadOnlyList<VertexBufferElement> GetElements()
        {
            return _elements;
        }

        /// <summary>
        /// It represents the size of the whole data row (sum of elements sizes)
        /// </summary>
        public int GetStride()
        {
            return _stride;
        }

        public VertexBufferLayout(List<VertexBufferElement> elements)
        {
            _elements = elements;
            CalculateOffsets();
        }

        private void CalculateOffsets()
        {
            int offset = 0;
            _stride = 0;
            foreach (var element in _elements)
            {
                element.Offset = offset;
                offset += element.Size;
                _stride += element.Size;
            }
        }
    }
}