using System;

namespace SCVE.Core.Rendering
{
    public class TextureData : IDisposable
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public byte[] RgbaPixels { get; set; }

        public TextureData(int width, int height, byte[] rgbaPixels)
        {
            Width = width;
            Height = height;
            RgbaPixels = rgbaPixels;
        }

        public void Dispose()
        {
        }
    }
}