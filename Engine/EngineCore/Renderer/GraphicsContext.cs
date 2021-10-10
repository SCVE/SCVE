using System;
using Engine.Platform.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine.EngineCore.Renderer
{
    public abstract class GraphicsContext
    {
        public abstract void Init();
        public abstract void SwapBuffers();

        public static unsafe GraphicsContext Create(void* window)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None: throw new ArgumentException("RendererAPI::None is currently not supported!");
                case RendererAPI.API.OpenGL:  return new OpenGLContext((Window*)window);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
    }
}