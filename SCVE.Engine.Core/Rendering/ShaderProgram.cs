using System;
using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Loading;
using SCVE.Engine.Core.Primitives;

namespace SCVE.Engine.Core.Rendering
{
    public abstract class ShaderProgram : IRenderEntity, IBindable, IDisposable
    {
        public int Id { get; protected set; }

        public abstract void AttachShader(Shader shader);

        public abstract void DetachShader(Shader shader);

        public abstract void Bind();

        public abstract void Unbind();

        public abstract void SetVector4(string name, float x, float y, float z, float w);
        public abstract void SetVector3(string name, float x, float y, float z);
        public abstract void SetMatrix4(string name, ScveMatrix4X4 matrix);
        public abstract void SetFloat(string name, float value);
        public abstract void SetInt(string name, int value);

        public abstract void Link();

        public abstract ShaderProgramBinaryData GetBinary();

        public abstract void Dispose();
    }
}