using System;
using SCVE.Core.Rendering;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.ImageSharpBindings
{
    public class ImageSharpTextureLoader : ITextureLoader
    {
        public byte[] Load(string path)
        {
            Image<Rgba32> image = Image.Load<Rgba32>(path);

            image.Mutate(i => i.Flip(FlipMode.Vertical));
            
            //Convert ImageSharp's format into a byte array, so we can use it with OpenGL.
            
            // 4 is because we store 4 colors (RGBA) for a pixel
            
            var pixels = new byte[4 * image.Width * image.Height];
            int index = 0;

            for (int y = 0; y < image.Height; y++) {
                var row = image.GetPixelRowSpan(y);

                for (int x = 0; x < image.Width; x++) {
                    pixels[index++] = row[x].R;
                    pixels[index++] = row[x].G;
                    pixels[index++] = row[x].B;
                    pixels[index++] = row[x].A;
                }
            }

            return pixels;
        }
    }
}