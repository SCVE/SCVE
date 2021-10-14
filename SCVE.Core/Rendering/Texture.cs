using System;

namespace SCVE.Core.Rendering
{
    public abstract class Texture : IRenderEntity, IDisposable
    {
        public int Id { get; protected set; }

        public abstract void Bind(int slot);
        
        public abstract void Unbind();
        
        public abstract void Dispose();
    }
}