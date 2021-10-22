using System;
using System.Numerics;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpTests
{
    class Program
    {
        static void Main(string[] args)
        {
            new Example().Run();
            
            return;
            
            FontCollection collection = new FontCollection();
            FontFamily family = collection.Install("assets/arial.ttf");
            Font font = family.CreateFont(14, FontStyle.Regular);
            
            var glyph = font.GetGlyph('A');

            var fontRectangle = glyph.BoundingBox(Vector2.Zero, new Vector2(72, 72));
            
            var backgroundColor = new Rgba32(0, 0, 0, 0);

            string text = "Bird Egop Is Super Cool";

            var size = TextMeasurer.Measure(text, new RendererOptions(font, 72));

            Image<Rgba32> image = new Image<Rgba32>((int)size.Width, (int)size.Height);
            image.Mutate(i => i.DrawText(text, font, Color.Black, PointF.Empty));

            var topPixel = GetTopPixel(image, backgroundColor);
            var bottomPixel = GetBottomPixel(image, backgroundColor);
            var leftPixel = GetLeftPixel(image, backgroundColor);
            var rightPixel = GetRightPixel(image, backgroundColor);

            image.Mutate(i => i.Crop(new Rectangle(leftPixel, topPixel, rightPixel - leftPixel, bottomPixel - topPixel)));

            image.Save("assets/texted.png");
        }

        private static int GetTopPixel(Image<Rgba32> image, Rgba32 backgroundColor)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    if (pixel != backgroundColor)
                    {
                        return y;
                    }
                }
            }

            throw new InvalidOperationException("Top pixel not found.");
        }

        private static int GetBottomPixel(Image<Rgba32> image, Rgba32 backgroundColor)
        {
            for (int y = image.Height - 1; y >= 0; y--)
            {
                for (int x = image.Width - 1; x >= 0; x--)
                {
                    Rgba32 pixel = image[x, y];
                    if (pixel != backgroundColor)
                    {
                        return y;
                    }
                }
            }

            throw new InvalidOperationException("Bottom pixel not found.");
        }

        private static int GetLeftPixel(Image<Rgba32> image, Rgba32 backgroundColor)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Rgba32 pixel = image[x, y];
                    if (pixel != backgroundColor)
                    {
                        return x;
                    }
                }
            }

            throw new InvalidOperationException("Left pixel not found.");
        }

        private static int GetRightPixel(Image<Rgba32> image, Rgba32 backgroundColor)
        {
            for (int x = image.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Rgba32 pixel = image[x, y];
                    if (pixel != backgroundColor)
                    {
                        return x;
                    }
                }
            }

            throw new InvalidOperationException("Left pixel not found.");
        }
    }
}