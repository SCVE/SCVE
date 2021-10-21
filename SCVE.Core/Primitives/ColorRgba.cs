using SCVE.Core.Utilities;

namespace SCVE.Core.Primitives
{
    public class ColorRgba
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

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
    }
}