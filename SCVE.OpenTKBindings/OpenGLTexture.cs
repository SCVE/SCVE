using OpenTK.Graphics.OpenGL;
using SCVE.Core.Loading;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLTexture : Texture
    {
        public OpenGLTexture(TextureData data)
        {
            Logger.Construct(nameof(OpenGLTexture));
            Width = data.Width;
            Height = data.Height;
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
                pixels: data.RgbaPixels
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