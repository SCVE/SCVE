using SCVE.Core.Utilities;

namespace SCVE.Core.Primitives
{
    public class Rect
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Rect()
        {
            Logger.Construct(nameof(Rect));
        }

        public Rect(float x, float y, float width, float height)
        {
            Logger.Construct(nameof(Rect));
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Deconstruct(out float x, out float y, out float width, out float height)
        {
            x      = X;
            y      = Y;
            width  = Width;
            height = Height;
        }
    }
}