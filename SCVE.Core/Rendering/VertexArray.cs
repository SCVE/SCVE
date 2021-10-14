using System;
using System.Collections.Generic;

namespace SCVE.Core.Rendering
{
    /// <summary>
    /// VAO
    /// </summary>
    public abstract class VertexArray : IDisposable
    {
        public int Id;

        public List<VertexBuffer> VertexBuffers;

        public IndexBuffer IndexBuffer;

        public VertexArray()
        {
            VertexBuffers = new();
        }

        public abstract void AddVertexBuffer(VertexBuffer vertexBuffer);

        public virtual void SetIndexBuffer(IndexBuffer indexBuffer)
        {
            IndexBuffer = indexBuffer;
        }

        /// <summary>
        /// When we bind vertex array (VAO), all of the used resources are bound automatically
        /// </summary>
        public abstract void Bind();

        public abstract void Unbind();
        
        public abstract void Dispose();
    }
}