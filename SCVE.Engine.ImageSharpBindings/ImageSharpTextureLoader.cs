using SCVE.Engine.Core.Loading;
using SCVE.Engine.Core.Loading.Loaders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Engine.ImageSharpBindings
{
    public class ImageSharpTextureLoader : ITextureLoader
    {
        public TextureFileData Load(string path, bool flip = true)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(path);

            if (flip)
            {
                image.Mutate(i => i.Flip(FlipMode.Vertical));
            }

            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.

            // 4 is because we store 4 colors (RGBA) for a pixel

            var pixels = ImageToBytes(image);

            return new TextureFileData(image.Width, image.Height, pixels);
        }

        public static byte[] ImageToBytes(Image<Rgba32> image)
        {
            var pixels = new byte[4 * image.Width * image.Height];
            
            image.CopyPixelDataTo(pixels);
            return pixels;
        }
    }
}