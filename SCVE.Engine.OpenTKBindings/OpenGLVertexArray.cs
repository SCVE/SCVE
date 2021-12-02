using OpenTK.Graphics.OpenGL;
using SCVE.Engine.Core;
using SCVE.Engine.Core.Misc;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.OpenTKBindings
{
    public class OpenGLVertexArray : VertexArray
    {
        private int _usedVertexBufferLocations = 0;
        public OpenGLVertexArray()
        {
            Logger.Construct(nameof(OpenGLVertexArray));
            Id = GL.GenVertexArray();
        }
        
        public override void AddVertexBuffer(VertexBuffer vertexBuffer)
        {
            Logger.Trace("OpenGLVertexArray.AddVertexBuffer()");
            var layout = vertexBuffer.Layout;

            if (layout is null)
            {
                throw new ScveException("No layout for vertexBuffer");
            }

            Bind();
            vertexBuffer.Bind();
            
            foreach (var vertexBufferElement in layout.GetElements())
            {
                GL.EnableVertexAttribArray(_usedVertexBufferLocations);
                GL.VertexAttribPointer(
                    _usedVertexBufferLocations,
                    vertexBufferElement.Type.ComponentCount(),
                    vertexBufferElement.Type.ToGLType(),
                    vertexBufferElement.Normalized,
                    layout.GetStride(),
                    vertexBufferElement.Offset);
                _usedVertexBufferLocations++;
            }
            
            VertexBuffers.Add(vertexBuffer);
        }

        public override void SetIndexBuffer(IndexBuffer indexBuffer)
        {
            Logger.Trace("OpenGLVertexArray.SetIndexBuffer()");
            Bind();
            indexBuffer.Bind();

            base.SetIndexBuffer(indexBuffer);
        }

        public override void Bind()
        {
            Logger.Trace("OpenGLVertexArray.Bind()");
            GL.BindVertexArray(Id);
        }

        public override void Unbind()
        {
            Logger.Trace("OpenGLVertexArray.Unbind()");
            GL.BindVertexArray(0);
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLVertexArray.Dispose()");
            GL.DeleteVertexArray(Id);
            
            foreach (var vertexBuffer in VertexBuffers)
            {
                vertexBuffer.Dispose();
            }
            
            IndexBuffer.Dispose();
        }
    }
}