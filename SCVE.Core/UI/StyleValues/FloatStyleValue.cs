namespace SCVE.Core.UI.StyleValues
{
    public class FloatStyleValue : StyleValue<float>
    {
        public FloatStyleValue() : base(0)
        {
        }

        public FloatStyleValue(float value) : base(value)
        {
        }

        public static implicit operator float(FloatStyleValue value)
        {
            return value.Value;
        }

        public static implicit operator FloatStyleValue(float value)
        {
            return new FloatStyleValue(value);
        }
    }
}