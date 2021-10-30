namespace SCVE.Core.UI
{
    public class StyleValue<T>
    {
        public T Value { get; }

        public StyleValue(T value)
        {
            Value = value;
        }

        public static implicit operator T(StyleValue<T> value)
        {
            return value.Value;
        }

        public static implicit operator StyleValue<T>(T value)
        {
            return new StyleValue<T>(value);
        }
    }
}