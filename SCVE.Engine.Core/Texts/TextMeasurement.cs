namespace SCVE.Engine.Core.Texts
{
    public class TextMeasurement
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public TextMeasurement(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}