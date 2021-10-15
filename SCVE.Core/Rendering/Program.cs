using System;

namespace SCVE.Core.Rendering
{
    public abstract class Program : IRenderEntity, IBindable, IDisposable
    {
        public int Id { get; protected set; }

        public abstract void AttachShader(Shader shader);
        
        public abstract void DetachShader(Shader shader);

        public abstract void Bind();
        
        public abstract void Unbind();

        public abstract void SetVector4(string name, float x, float y, float z, float w);

        public abstract void Link();

        public abstract void Dispose();
    }
}