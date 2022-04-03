using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Engine.ImageSharpBindings
{
    public class ImageSharpImageManipulator
    {
        public static void DrawOnTop(byte[] srcPixels, int srcWidth, int srcHeight, byte[] imagePixels, int imageWidth, int imageHeight)
        {
            using var srcImage = Image.WrapMemory<Rgba32>(srcPixels, srcWidth, srcHeight);

            using var image = Image.WrapMemory<Rgba32>(imagePixels, imageWidth, imageHeight);

            srcImage.Mutate(i => i.DrawImage(image, new Point(0, 0), 1f));
        }
    }
}