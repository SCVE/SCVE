using System;

namespace SCVE.Core.Rendering
{
    public abstract class Texture : IRenderEntity, IBindable, IDisposable
    {
        public int Id { get; protected set; }

        public abstract void Bind();
        
        public abstract void Unbind();
        
        public abstract void Dispose();
    }
}