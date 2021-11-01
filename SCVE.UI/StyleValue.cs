namespace SCVE.UI
{
    public class StyleValue<T>
    {
        public T Value { get; set; }

        public bool Specified { get; set; }

        public StyleValue(T value, bool specified = false)
        {
            Value     = value;
            Specified = specified;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}