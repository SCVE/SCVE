using SCVE.Core.Primitives;

namespace SCVE.Core.UI.StyleValues
{
    public class ColorStyleValue : StyleValue<ColorRgba>
    {
        public ColorStyleValue() : base(ColorRgba.White)
        {
        }

        public ColorStyleValue(ColorRgba value) : base(value)
        {
        }

        public static implicit operator ColorRgba(ColorStyleValue value)
        {
            return value.Value;
        }

        public static implicit operator ColorStyleValue(ColorRgba value)
        {
            return new ColorStyleValue(value);
        }
    }
}