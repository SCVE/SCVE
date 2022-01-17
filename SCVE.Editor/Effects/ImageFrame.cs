using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SCVE.Editor.Effects
{
    public class ImageFrame
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public byte[] RawBytes { get; private set; }

        public Image<Rgba32> ImageSharpImage { get; private set; }

        public ImageFrame(int width, int height)
        {
            Width    = width;
            Height   = height;
            RawBytes = new byte[width * height * 4];
        }

        public void CreateImageSharpWrapper()
        {
            ImageSharpImage = Image.WrapMemory<Rgba32>(RawBytes, Width, Height);
        }
    }
}