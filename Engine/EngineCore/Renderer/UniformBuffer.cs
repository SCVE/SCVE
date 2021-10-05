using System;
using Engine.Platform.OpenGL;

namespace Engine.EngineCore.Renderer
{
    public abstract class UniformBuffer
    {
        public abstract unsafe void SetData(void* data, uint size, uint offset = 0);
        
        public static UniformBuffer Create(uint size, uint offset)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None: throw new ArgumentException("RendererAPI::None is currently not supported!");
                case RendererAPI.API.OpenGL:  return new OpenGLUniformBuffer(size, offset);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
    }
}