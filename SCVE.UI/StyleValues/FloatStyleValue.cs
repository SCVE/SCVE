namespace SCVE.UI.StyleValues
{
    public class FloatStyleValue : StyleValue<float>
    {
        public bool IsAbsolute { get; set; } = true;

        public bool IsRelative { get; set; }
        
        public FloatStyleValue() : base(0)
        {
        }

        public FloatStyleValue(float value) : base(value)
        {
        }

        public float Flatten(float origin)
        {
            if (IsRelative)
            {
                return origin * Value / 100;
            }
            else
            {
                return Value;
            }
        }
    }
}