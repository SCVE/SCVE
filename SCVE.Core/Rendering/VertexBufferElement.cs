namespace SCVE.Core.Rendering
{
    /// <summary>
    /// This object describes a single vertex element data
    /// </summary>
    public class VertexBufferElement
    {
        public string Name;
        public VertexBufferElementType Type;
        public int Size;
        public int Offset;
        public bool Normalized;

        public VertexBufferElement(VertexBufferElementType type, string name, bool normalized = false)
        {
            Name = name;
            Type = type;
            Size = Type.SizeInBytes();
            Normalized = normalized;
        }
    }
}