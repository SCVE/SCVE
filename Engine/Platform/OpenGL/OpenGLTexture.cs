using System;
using System.IO;
using System.Runtime.InteropServices;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Engine.Platform.OpenGL
{
    public class OpenGLTexture2D : Texture2D
    {
        public OpenGLTexture2D(uint width, uint height)
        {
            _internalFormat = SizedInternalFormat.Rgba8;
            _dataFormat = PixelFormat.Rgba;

            _rendererId = GL.CreateTexture(TextureTarget.Texture2d);
            GL.TextureStorage2D(_rendererId, 1, _internalFormat, (int)_width, (int)_height);

            GL.TextureParameteri(_rendererId, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameteri(_rendererId, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TextureParameteri(_rendererId, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameteri(_rendererId, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public OpenGLTexture2D(string path)
        {
            // Load the image
            var image = Image.Load(path);

            // ImageSharp loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            // This will correct that, making the texture display properly.
            image.Mutate(x => x.Flip(FlipMode.Vertical));

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.

            using var memoryStream = new MemoryStream();
            var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(PngFormat.Instance);
            image.Save(memoryStream, imageEncoder);
            
            var pixels = memoryStream.ToArray();
            var handle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            
            _width = (uint)image.Width;
            _height = (uint)image.Height;
            
            // TODO: Figure out how SixLabors loads different images
            _internalFormat = SizedInternalFormat.Rgba8;
            _dataFormat = PixelFormat.Rgba;

            if (((int)_internalFormat & (int)_dataFormat) != 0)
            {
                throw new ArgumentException("Format not supported!");
            }

            _rendererId = GL.CreateTexture(TextureTarget.Texture2d);
            
            GL.TextureStorage2D(_rendererId, 1, _internalFormat, (int)_width, (int)_height);
            
            GL.TextureParameteri(_rendererId, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameteri(_rendererId, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TextureParameteri(_rendererId, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TextureParameteri(_rendererId, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            
            GL.TextureSubImage2D(_rendererId, 0,0,0, (int)_width, (int)_height, _dataFormat, PixelType.UnsignedByte, handle.AddrOfPinnedObject());
            handle.Free();
        }

        ~OpenGLTexture2D()
        {
            GL.DeleteTexture(_rendererId);
        }
        
        public override uint GetWidth()
        {
            return _width;
        }

        public override uint GetHeight()
        {
            return _height;
        }

        public override uint GetRendererId()
        {
            return (uint)_rendererId.Handle;
        }

        public override unsafe void SetData(void* data, uint size)
        {
            uint bpp = _dataFormat == PixelFormat.Rgba ? 4u : 3u;
            if (size != _width * _height * bpp)
            {
                throw new ArgumentException("Data must be entire texture!");
            }
            GL.TextureSubImage2D(_rendererId, 0, 0, 0, (int)_width, (int)_height, _dataFormat, PixelType.UnsignedByte, data);
        }

        public override void Bind(uint slot)
        {
            GL.BindTextureUnit(slot, _rendererId);
        }

        public override bool IsLoaded()
        {
            return _isLoaded;
        }

        public static bool operator==(OpenGLTexture2D texture1, Texture texture2)
        {
            return texture1._rendererId.Handle == texture2.GetRendererId();
        }

        public static bool operator!=(OpenGLTexture2D texture1, Texture texture2)
        {
            return !(texture1 == texture2);
        }

        private string _path;
        private bool _isLoaded;
        private uint _width;
        private uint _height;
        private TextureHandle _rendererId;
        private SizedInternalFormat _internalFormat;
        private PixelFormat _dataFormat;
    }
}