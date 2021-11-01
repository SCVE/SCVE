using SCVE.Core.Primitives;

namespace SCVE.UI.StyleValues
{
    public class ColorStyleValue : StyleValue<ColorRgba>
    {
        public ColorStyleValue() : base(ColorRgba.White)
        {
        }

        public ColorStyleValue(ColorRgba value) : base(value)
        {
        }
    }
}