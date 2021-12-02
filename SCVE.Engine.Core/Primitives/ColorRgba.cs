using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.Core.Primitives
{
    public class ColorRgba
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public static readonly ColorRgba White = new(1, 1, 1, 1);

        public ColorRgba()
        {
            Logger.Construct(nameof(ColorRgba));
        }

        public ColorRgba(float r, float g, float b, float a)
        {
            Logger.Construct(nameof(ColorRgba));
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public ColorRgba(ColorRgba color)
        {
            Logger.Construct(nameof(ColorRgba));
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }

        public override string ToString()
        {
            return $"ColorRgba{{{nameof(R)}: {R}, {nameof(G)}: {G}, {nameof(B)}: {B}, {nameof(A)}: {A}}}";
        }
    }
}