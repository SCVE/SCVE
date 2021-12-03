using System;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.Core.Loading
{
    public class TextureFileData : IDisposable
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public byte[] RgbaPixels { get; set; }

        public TextureFileData(int width, int height, byte[] rgbaPixels)
        {
            Logger.Construct(nameof(TextureFileData));
            Width = width;
            Height = height;
            RgbaPixels = rgbaPixels;
        }

        public void Dispose()
        {
        }
    }
}