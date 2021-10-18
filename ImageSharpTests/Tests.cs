using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpTests
{
    public class Tests
    {
        public void GenerateFontTextureAtlas()
        {
            // image.Mutate(i => i.DrawText("Some text", font, Color.White, PointF.Empty));
        }

        public static void Test2()
        {
            FontCollection collection = new FontCollection();
            FontFamily family = collection.Install("assets/arial.ttf");
            Font font = family.CreateFont(72, FontStyle.Regular);

            string text = "Hello World Hello World Hello World Hello World Hello World";

            var glyphs = TextBuilder.GenerateGlyphs(text, PointF.Empty, new RendererOptions(font, 72));

            Image<Rgba32> image = new Image<Rgba32>(1024, 1024);

            var textOptions = new TextOptions()
            {
                WrapTextWidth = 1024
            };

            var drawingOptions = new DrawingOptions()
            {
                TextOptions = textOptions
            };
            
            
            image.Mutate(i => i.DrawText(drawingOptions, text, font, Color.Black, PointF.Empty));


            image.SaveAsPng("assets/output.png");
        }
    }
}