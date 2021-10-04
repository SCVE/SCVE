using System;

namespace Engine.EngineCore.Renderer
{
    public abstract class Texture
    {
        public abstract uint GetWidth();
        public abstract uint GetHeight();
        public abstract uint GetRendererId();

        public abstract unsafe void SetData(void* data, uint size);

        public abstract void Bind(uint slot);

        public abstract bool IsLoaded();

        public static bool operator==(Texture left, Texture right)
        {
            return false;
        }

        public static bool operator!=(Texture left, Texture right)
        {
            return false;
        }
    }

    public abstract class Texture2D : Texture
    {
        public static Texture2D Create(uint width, uint height)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None:    throw new ArgumentException("RendererAPI::None is currently not supported!"); 
                case RendererAPI.API.OpenGL:  return new OpenGLTexture2D(width, height);
                default: throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
        
        public static Texture2D Create(string path)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None:    throw new ArgumentException("RendererAPI::None is currently not supported!"); 
                case RendererAPI.API.OpenGL:  return new OpenGLTexture2D(path);
                default: throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
    }
}