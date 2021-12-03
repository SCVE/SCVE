using OpenTK.Graphics.OpenGL;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.OpenTKBindings
{
    public class OpenGLIndexBuffer : IndexBuffer
    {
        public OpenGLIndexBuffer()
        {
            Logger.Construct(nameof(OpenGLIndexBuffer));
        }

        public OpenGLIndexBuffer(int[] indices, BufferUsage usage)
        {
            Logger.Construct(nameof(OpenGLIndexBuffer));
            Count = indices.Length;
            Id = GL.GenBuffer();
            Upload(indices, usage);
        }

        public override void Upload(int[] indices, BufferUsage usage)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
            GL.BufferData(BufferTarget.ArrayBuffer, indices.Length * sizeof(int), indices, usage.ToOpenGlUsage());
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