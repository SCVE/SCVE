using System;

namespace SCVE.Engine.Core.Rendering
{
    public abstract class Shader : IRenderEntity, IDisposable
    {
        public int Id { get; protected set; }

        public abstract void Compile();

        public abstract void Dispose();
    }
}