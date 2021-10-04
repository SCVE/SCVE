using System;
using System.Collections.Generic;

namespace Engine.EngineCore.Renderer
{
    public enum FramebufferTextureFormat
    {
        None = 0,

        // Color
        RGBA8,
        RED_INTEGER,

        // Depth/stencil
        DEPTH24STENCIL8,

        // Defaults
        Depth = DEPTH24STENCIL8
    }

    public struct FramebufferTextureSpecification
    {
        public FramebufferTextureSpecification(FramebufferTextureFormat format = FramebufferTextureFormat.None)
        {
            TextureFormat = format;
        }

        public FramebufferTextureFormat TextureFormat;
        // TODO: filtering/wrap
    }

    public struct FramebufferAttachmentSpecification
    {
        public FramebufferAttachmentSpecification(IList<FramebufferTextureSpecification> attachments)
        {
            Attachments = attachments;
        }

        public IList<FramebufferTextureSpecification> Attachments;
    }

    public struct FramebufferSpecification
    {
        public FramebufferSpecification(FramebufferAttachmentSpecification attachments, uint width = 0, uint height = 0, uint samples = 0, bool swapChainTarget = false)
        {
            Width = width;
            Height = height;
            Attachments = attachments;
            Samples = samples;
            SwapChainTarget = swapChainTarget;
        }

        public uint Width;
        public uint Height;
        public FramebufferAttachmentSpecification Attachments;
        public uint Samples;

        public bool SwapChainTarget;
    };

    public abstract class Framebuffer
    {
        public abstract void Bind();
        public abstract void Unbind();
        public abstract void Resize(uint width, uint height);
        public abstract int ReadPixel(uint attachmentIndex, int x, int y);
        public abstract void ClearAttachment(uint attachmentIndex, int value);
        public abstract uint GetColorAttachmentRendererId(uint index = 0);
        public abstract FramebufferSpecification GetSpecification();

        public static Framebuffer Create(FramebufferSpecification spec)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None: throw new ArgumentException("RendererAPI::None is currently not supported!");
                case RendererAPI.API.OpenGL:  return new OpenGLFramebuffer(spec);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI!");
            }
        }
    }
}