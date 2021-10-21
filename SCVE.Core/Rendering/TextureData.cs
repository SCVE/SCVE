using System;
using SCVE.Core.Utilities;

namespace SCVE.Core.Rendering
{
    public class TextureData : IDisposable
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public byte[] RgbaPixels { get; set; }

        public TextureData(int width, int height, byte[] rgbaPixels)
        {
            Logger.Construct(nameof(TextureData));
            Width = width;
            Height = height;
            RgbaPixels = rgbaPixels;
        }

        public void Dispose()
        {
        }
    }
}