using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLIndexBuffer : IndexBuffer
    {
        public OpenGLIndexBuffer(int[] indices)
        {
            Logger.Trace("Constructing OpenGLIndexBuffer");
            Count = indices.Length;
            Id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
        }
        
        public override void Bind()
        {
            Logger.Trace("OpenGLIndexBuffer.Bind()");
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Id);
        }

        public override void Unbind()
        {
            Logger.Trace("OpenGLIndexBuffer.Unbind()");
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLIndexBuffer.Dispose()");
            GL.DeleteBuffer(Id);
        }
    }
}