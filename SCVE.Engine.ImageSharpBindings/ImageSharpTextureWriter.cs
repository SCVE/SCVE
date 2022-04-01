using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SCVE.Engine.ImageSharpBindings
{
    public class ImageSharpTextureWriter
    {
        public void Save(byte[] rgbaPixels, int width, int height, string destPath)
        {
            using var image = Image.WrapMemory<Rgba32>(rgbaPixels, width, height);
            
            image.SaveAsPng(destPath);
        }
    }
}