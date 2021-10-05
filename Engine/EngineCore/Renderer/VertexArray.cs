using System;
using System.Collections.Generic;
using Engine.Platform.OpenGL;

namespace Engine.EngineCore.Renderer
{
    public abstract class VertexArray
    {
        public abstract void Bind();
        public abstract void Unbind();
        public abstract void AddVertexBuffer(VertexBuffer vertexBuffer);
        public abstract void SetIndexBuffer(IndexBuffer indexBuffer);

        public abstract IList<VertexBuffer> GetVertexBuffers();

        public abstract IndexBuffer GetIndexBuffer();

        public static VertexArray Create()
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None: throw new ArgumentException("RendererAPI::None is currently not supported!");
                case RendererAPI.API.OpenGL:  return new OpenGLVertexArray();
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
    }
}