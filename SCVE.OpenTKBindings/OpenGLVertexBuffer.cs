using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLVertexBuffer : VertexBuffer
    {
        private int _size;
        public OpenGLVertexBuffer(float[] vertices)
        {
            Logger.Construct(nameof(OpenGLVertexBuffer));
            Id = GL.GenBuffer();
            _size = sizeof(float) * vertices.Length;
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, _size, vertices, BufferUsageHint.StaticDraw);
        }
        
        public OpenGLVertexBuffer(int size)
        {
            Logger.Construct(nameof(OpenGLVertexBuffer));
            Id = GL.GenBuffer();
            _size = size;
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, _size, IntPtr.Zero, BufferUsageHint.StaticDraw);
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