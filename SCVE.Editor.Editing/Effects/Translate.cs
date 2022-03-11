using SCVE.Editor.Editing.Visitors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Editing.Effects
{
    public class Translate : EffectBase
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Translate()
        {
        }

        protected override void Algorithm(byte[] pixels, int width, int height)
        {
            using var srcImageSharpImage = Image.WrapMemory<Rgba32>(pixels, width, height);

            var clone = srcImageSharpImage.Clone();

            srcImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

            srcImageSharpImage.Mutate(i => i.DrawImage(clone, new Point(X, Y), 1));
        }

        public override void AcceptVisitor(IEffectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}