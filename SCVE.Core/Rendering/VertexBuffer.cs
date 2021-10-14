using System;

namespace SCVE.Core.Rendering
{
    /// <summary>
    /// VBO
    /// </summary>
    public abstract class VertexBuffer : IDisposable
    {
        public int Id;
        
        public VertexBufferLayout Layout { get; set; }
        
        public abstract int GetSize();

        public abstract void Bind();

        public abstract void Unbind();

        public abstract void Dispose();
    }
}