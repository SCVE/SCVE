using SCVE.Editor.Editing;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor
{
    public class SequenceSampler
    {
        public Image<Rgba32> PreviewImage;

        FontCollection fontCollection = new FontCollection();
        private Font font;

        public SequenceSampler()
        {
            fontCollection.Install("assets/Font/arial.ttf");
            font         = fontCollection.CreateFont("arial", 72);
            PreviewImage = new Image<Rgba32>(30, 30);
        }

        public void Sample(Sequence sequence, int timeFrame)
        {
            var image = new Image<Rgba32>((int)sequence.Resolution.X, (int)sequence.Resolution.Y);

            image.Mutate(i => i.Fill(Color.Black));
            image.Mutate(i => i.DrawText($"{timeFrame}", font, Color.White, new PointF(900, 450)));

            PreviewImage = image;
        }
    }
}