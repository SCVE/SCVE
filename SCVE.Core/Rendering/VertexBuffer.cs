using System;

namespace SCVE.Core.Rendering
{
    public abstract class VertexBuffer : IDisposable
    {
        protected int Id;

        protected VertexBuffer(float[] vertices)
        {
        }

        protected VertexBuffer(int size)
        {
        }

        public abstract void Bind();

        public abstract void Unbind();

        public abstract void Dispose();
    }
}