using System;

namespace SCVE.Core.Rendering
{
    public abstract class Shader : IRenderEntity, IDisposable
    {
        public int Id { get; protected set; }

        public abstract void Compile();

        public abstract void Dispose();
    }
}