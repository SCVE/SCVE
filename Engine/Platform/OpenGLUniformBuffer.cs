using System;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Platform
{
    public class OpenGLUniformBuffer : UniformBuffer
    {
        public OpenGLUniformBuffer(uint size, uint binding)
        {
            _rendererID = GL.CreateBuffer();
            GL.NamedBufferData(_rendererID, (nint)size, IntPtr.Zero, VertexBufferObjectUsage.DynamicDraw); // TODO: investigate usage hint
            GL.BindBufferBase(BufferTargetARB.UniformBuffer, binding, _rendererID);
        }

        ~OpenGLUniformBuffer()
        {
            GL.DeleteBuffer(_rendererID);
        }

        public override unsafe void SetData(void* data, uint size, uint offset = 0)
        {
            GL.NamedBufferSubData(_rendererID, (nint)offset, (nint)size, data);
        }

        private readonly BufferHandle _rendererID;
    }
}