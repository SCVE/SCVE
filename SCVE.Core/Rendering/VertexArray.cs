using System;

namespace SCVE.Core.Rendering
{
    /// <summary>
    /// VAO
    /// </summary>
    public abstract class VertexArray : IDisposable
    {
        public int Id;

        public abstract void AddVertexBuffer(VertexBuffer vertexBuffer);
        
        public abstract void AddIndexBuffer(IndexBuffer indexBuffer);

        public abstract void Bind();

        public abstract void Unbind();
        
        public abstract void Dispose();
    }
}