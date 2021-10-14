namespace SCVE.Core.Rendering
{
    /// <summary>
    /// This object describes a single vertex element data
    /// <remarks>
    /// Should only be used in <see cref="VertexBufferLayout"/>
    /// </remarks>
    /// </summary>
    public class VertexBufferElement
    {
        public string Name { get; private set; }
        public VertexBufferElementType Type { get; private set; }
        public int Size { get; private set; }
        public int Offset { get; set; }
        public bool Normalized { get; private set; }

        public VertexBufferElement(VertexBufferElementType type, string name, bool normalized = false)
        {
            Name = name;
            Type = type;
            Size = Type.SizeInBytes();
            Normalized = normalized;
        }
    }
}