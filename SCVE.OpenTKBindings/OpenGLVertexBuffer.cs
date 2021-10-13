using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public class OpenGLVertexBuffer : VertexBuffer
    {
        public OpenGLVertexBuffer(float[] vertices) : base(vertices)
        {
            Id = GL.GenBuffer();
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length, vertices, BufferUsageHint.StaticDraw);
        }
        
        public OpenGLVertexBuffer(int size) : base(size)
        {
            Id = GL.GenBuffer();
            GL.BufferData(BufferTarget.ArrayBuffer, size, IntPtr.Zero, BufferUsageHint.StaticDraw);
        }

        public override void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
        }

        public override void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public override void Dispose()
        {
            GL.DeleteBuffer(Id);
        }
    }
}