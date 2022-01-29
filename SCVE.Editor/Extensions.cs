using System;

namespace SCVE.Editor
{
    public static class Extensions
    {
        public static bool IsWithinExclusive<T>(this T value, T minimum, T maximum) where T : IComparable<T> {
            if (value.CompareTo(minimum) <= 0)
                return false;
            if (value.CompareTo(maximum) >= 0)
                return false;
            return true;
        }
        public static bool IsWithinInclusive<T>(this T value, T minimum, T maximum) where T : IComparable<T> {
            if (value.CompareTo(minimum) < 0)
                return false;
            if (value.CompareTo(maximum) > 0)
                return false;
            return true;
        }
    }
}