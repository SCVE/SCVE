using System;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Platform.OpenGL
{
    public class Utils
    {
        public static TextureTarget TextureTarget(bool multisampled)
        {
            return multisampled ? OpenTK.Graphics.OpenGL.TextureTarget.Texture2dMultisample : OpenTK.Graphics.OpenGL.TextureTarget.Texture2d;
        }

        public static TextureHandle CreateTexture(bool multisampled)
        {
            return GL.CreateTexture(TextureTarget(multisampled));
        }

        public static TextureHandle[] CreateTextures(bool multisampled, int count)
        {
            TextureHandle[] handles = new TextureHandle[count];
            GL.CreateTextures(TextureTarget(multisampled), handles);
            return handles;
        }

        public static void BindTexture(bool multisampled, TextureHandle id)
        {
            GL.BindTexture(TextureTarget(multisampled), id);
        }

        public static void AttachColorTexture(TextureHandle id, int samples, InternalFormat internalFormat, PixelFormat format, uint width, uint height, int index)
        {
            bool multisampled = samples > 1;
            if (multisampled)
            {
                GL.TexImage2DMultisample(OpenTK.Graphics.OpenGL.TextureTarget.Texture2dMultisample, samples, internalFormat, (int)width, (int)height, false);
            }
            else
            {
                GL.TexImage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, 0, (int)internalFormat, (int)width, (int)height, 0, format, PixelType.UnsignedByte, IntPtr.Zero);

                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, (FramebufferAttachment)((uint)FramebufferAttachment.ColorAttachment0 + index), TextureTarget(multisampled), id, 0);
        }

        public static void AttachDepthTexture(TextureHandle id, int samples, InternalFormat format, FramebufferAttachment attachmentType, uint width, uint height)
        {
            bool multisampled = samples > 1;
            if (multisampled)
            {
                GL.TexImage2DMultisample(OpenTK.Graphics.OpenGL.TextureTarget.Texture2dMultisample, samples, format, (int)width, (int)height, false);
            }
            else
            {
                GL.TexStorage2D(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, 1, (SizedInternalFormat)format, (int)width, (int)height);

                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameteri(OpenTK.Graphics.OpenGL.TextureTarget.Texture2d, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, attachmentType, TextureTarget(multisampled), id, 0);
        }

        public static bool IsDepthFormat(FramebufferTextureFormat format)
        {
            switch (format)
            {
                case FramebufferTextureFormat.DEPTH24STENCIL8: return true;
            }

            return false;
        }

        public static PixelFormat HazelFBTextureFormatToGL(FramebufferTextureFormat format)
        {
            switch (format)
            {
                case FramebufferTextureFormat.RGBA8: return PixelFormat.Rgba;
                case FramebufferTextureFormat.RED_INTEGER: return PixelFormat.RedInteger;
            }

            throw new ArgumentException("Unknown format");
        }
    }
}