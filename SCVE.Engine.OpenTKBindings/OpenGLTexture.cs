﻿using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Engine.Core.Loading;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.OpenTKBindings
{
    public class OpenGLTexture : Texture
    {
        public OpenGLTexture(TextureFileData fileData)
        {
            Logger.Construct(nameof(OpenGLTexture));
            Width = fileData.Width;
            Height = fileData.Height;
            Id = GL.GenTexture();
            
            // Hazel implementation
            // GL.TextureStorage2D(Id, 1, SizedInternalFormat.Rgba8, data.Width, data.Height);
            //
            // GL.TextureSubImage2D(Id, 0, 0, 0, data.Width, data.Height, PixelFormat.Rgba, PixelType.UnsignedByte, data.RgbaPixels);

            GL.BindTexture(TextureTarget.Texture2D, Id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            
            GL.TexImage2D(
                target: TextureTarget.Texture2D,
                level: 0,
                internalformat: PixelInternalFormat.Rgba,
                width: Width,
                height: Height,
                border: 0,
                format: PixelFormat.Rgba,
                type: PixelType.UnsignedByte,
                pixels: fileData.RgbaPixels
            );
        }
        public OpenGLTexture(int width, int height, IntPtr pixels)
        {
            Logger.Construct(nameof(OpenGLTexture));
            Width = width;
            Height = height;
            Id = GL.GenTexture();
            
            // Hazel implementation
            // GL.TextureStorage2D(Id, 1, SizedInternalFormat.Rgba8, data.Width, data.Height);
            //
            // GL.TextureSubImage2D(Id, 0, 0, 0, data.Width, data.Height, PixelFormat.Rgba, PixelType.UnsignedByte, data.RgbaPixels);

            GL.BindTexture(TextureTarget.Texture2D, Id);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            
            GL.TexImage2D(
                target: TextureTarget.Texture2D,
                level: 0,
                internalformat: PixelInternalFormat.Rgba,
                width: Width,
                height: Height,
                border: 0,
                format: PixelFormat.Rgba,
                type: PixelType.UnsignedByte,
                pixels: pixels
            );
        }

        public override void Bind(int slot)
        {
            // Hazel implementation
            // GL.BindTextureUnit(slot, Id);
            
            GL.ActiveTexture(TextureUnit.Texture0 + slot);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public override void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public override void Dispose()
        {
            GL.DeleteTexture(Id);
        }
    }
}