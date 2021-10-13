using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public class OpenGLIndexBuffer : IndexBuffer
    {
        public OpenGLIndexBuffer(int[] indices)
        {
            Id = GL.GenBuffer();
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, indices.Length, indices, BufferUsageHint.StaticDraw);
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