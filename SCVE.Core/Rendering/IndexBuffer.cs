using System;

namespace SCVE.Core.Rendering
{
    public abstract class IndexBuffer : IDisposable
    {
        public int Id;

        public abstract void Bind();

        public abstract void Unbind();

        public abstract void Dispose();
    }
}