using System;
using Silk.NET.OpenGL;

namespace SCVE.Editor.Imaging
{
    public class GpuImage : IImage
    {
        private Texture _texture;

        public int Width { get; set; }

        public int Height { get; set; }

        public uint GpuId => _texture.GlTexture; 

        /// <summary>
        /// Constructs a new image from an existing 
        /// </summary>
        public GpuImage(IImage image)
        {
            Width    = image.Width;
            Height   = image.Height;
            _texture = new Texture(EditorApp.Instance.GL, Width, Height, image.ToByteArray(), PixelFormat.Rgba);
        }

        public byte[] ToByteArray()
        {
            return _texture.DownloadData();
        }

        public void Dispose()
        {
            _texture?.Dispose();
        }
    }
}