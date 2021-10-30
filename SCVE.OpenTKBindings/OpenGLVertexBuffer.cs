using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLVertexBuffer : VertexBuffer
    {
        private int _size;

        public OpenGLVertexBuffer()
        {
            Logger.Construct(nameof(OpenGLVertexBuffer));
            Id = GL.GenBuffer();
        }
        
        public OpenGLVertexBuffer(float[] vertices, BufferUsage usage)
        {
            Logger.Construct(nameof(OpenGLVertexBuffer));
            Id = GL.GenBuffer();
            Upload(vertices, usage);
        }
        
        public OpenGLVertexBuffer(int size, BufferUsage usage)
        {
            Logger.Construct(nameof(OpenGLVertexBuffer));
            Id = GL.GenBuffer();
            _size = size;
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, _size, IntPtr.Zero, usage.ToOpenGlUsage());
        }

        public override void Upload(float[] data, BufferUsage usage)
        {
            _size = sizeof(float) * data.Length;
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, _size, data, usage.ToOpenGlUsage());
        }

        public override int GetSize()
        {
            return _size;
        }

        public override void Bind()
        {
            Logger.Trace("OpenGLVertexBuffer.Bind()");
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        }

        public override void Unbind()
        {
            Logger.Trace("OpenGLVertexBuffer.Unbind()");
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLVertexBuffer.Dispose()");
            GL.DeleteBuffer(Id);
        }
    }
}