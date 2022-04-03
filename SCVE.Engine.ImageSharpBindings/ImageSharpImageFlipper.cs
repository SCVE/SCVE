using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Engine.ImageSharpBindings
{
    public class ImageSharpImageFlipper
    {
        public static void FlipY(byte[] rgbaPixels, int width, int height)
        {
            using var image = Image.WrapMemory<Rgba32>(rgbaPixels, width, height);
            image.Mutate(i => i.Flip(FlipMode.Vertical));
        }
    }
}