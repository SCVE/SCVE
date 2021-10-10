using System;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Platform.OpenGL
{
    public class OpenGLVertexBuffer : VertexBuffer
    {
        public OpenGLVertexBuffer(uint size)
        {
            _rendererId = GL.CreateBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _rendererId);
            GL.BufferData(BufferTargetARB.ArrayBuffer, (nint)size, IntPtr.Zero, BufferUsageARB.DynamicDraw);
        }

        public unsafe OpenGLVertexBuffer(float* vertices, uint size)
        {
            _rendererId = GL.CreateBuffer();
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _rendererId);
            GL.BufferData(BufferTargetARB.ArrayBuffer, (nint)size, vertices, BufferUsageARB.DynamicDraw);
        }

        ~OpenGLVertexBuffer()
        {
            GL.DeleteBuffer(_rendererId);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _rendererId);
        }

        public override void Unbind()
        {
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, BufferHandle.Zero);
        }

        public override unsafe void SetData(void* data, uint size)
        {
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _rendererId);
            GL.BufferSubData(BufferTargetARB.ArrayBuffer, (nint)0, (nint)size, data);
        }

        public override BufferLayout GetLayout()
        {
            return _layout;
        }

        public override void SetLayout(BufferLayout layout)
        {
            _layout = layout;
        }

        private BufferHandle _rendererId;
        private BufferLayout _layout;
    }

    class OpenGLIndexBuffer : IndexBuffer
    {
        public unsafe OpenGLIndexBuffer(uint* indices, uint count)
        {
            _rendererId = GL.CreateBuffer();
            _count = count;
		
            // GL_ELEMENT_ARRAY_BUFFER is not valid without an actively bound VAO
            // Binding with GL_ARRAY_BUFFER allows the data to be loaded regardless of VAO state. 
            GL.BindBuffer(BufferTargetARB.ArrayBuffer, _rendererId);
            GL.BufferData(BufferTargetARB.ArrayBuffer, (nint)count * sizeof(uint), indices, BufferUsageARB.StaticDraw);
        }

        ~OpenGLIndexBuffer()
        {
            GL.DeleteBuffer(_rendererId);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, _rendererId);
        }

        public override void Unbind()
        {
            GL.BindBuffer(BufferTargetARB.ElementArrayBuffer, BufferHandle.Zero);
        }

        public override uint GetCount()
        {
            return _count;
        }

        private BufferHandle _rendererId;
        private uint _count;
    };
}