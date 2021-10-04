using System;
using Engine.Platform;
using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public abstract class RendererAPI
    {
        public enum API
        {
            None = 0,
            OpenGL = 1,
        }

        public abstract void Init();
        public abstract void SetViewport(uint x, uint y, uint width, uint height);
        public abstract void SetClearColor(Vector4 color);
        public abstract void Clear();
        public abstract void DrawIndexed(VertexArray vertexArray, uint indexCount);

        public static API GetAPI()
        {
            return _api;
        }

        public static RendererAPI Create()
        {
            switch (_api)
            {
                case API.None:    throw new ArgumentException("RendererAPI::None is currently not supported!"); 
                case API.OpenGL:  return new OpenGLRendererAPI();
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI");
            }
        }

        private static API _api;
    }
}