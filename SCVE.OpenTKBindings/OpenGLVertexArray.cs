using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public class OpenGLVertexArray : VertexArray
    {
        private int _usedVertexBufferCount = 0;
        
        public OpenGLVertexArray()
        {
            Id = GL.GenVertexArray();
        }
        
        public override void AddVertexBuffer(VertexBuffer vertexBuffer)
        {
            GL.EnableVertexAttribArray(_usedVertexBufferCount);
            // GL.VertexAttribPointer(vertexBuffer.Id, );
        }

        public override void AddIndexBuffer(IndexBuffer indexBuffer)
        {
            throw new System.NotImplementedException();
        }

        public override void Bind()
        {
            GL.BindVertexArray(Id);
        }

        public override void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(Id);
        }
    }
}