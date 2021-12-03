using System;

namespace SCVE.Engine.Core.Rendering
{
    public abstract class Texture : IRenderEntity, IDisposable
    {
        public int Id { get; protected set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public abstract void Bind(int slot);

        public abstract void Unbind();

        public abstract void Dispose();
    }
}