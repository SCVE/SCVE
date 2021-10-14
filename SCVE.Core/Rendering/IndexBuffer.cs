using System;

namespace SCVE.Core.Rendering
{
    /// <summary>
    /// An array of indices of vertex, that forms triangles
    /// </summary>
    public abstract class IndexBuffer : IRenderEntity, IDisposable
    {
        public int Id { get; protected set; }

        public int Count;

        public abstract void Bind();

        public abstract void Unbind();

        public abstract void Dispose();
    }
}