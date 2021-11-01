using System;
using SCVE.Core.Misc;

namespace SCVE.Core.Utilities
{
    public class Maths
    {
        /// <summary>
        /// Returns first higher power of 2
        /// <remarks>
        /// Max value is 8192
        /// </remarks>
        /// </summary>
        public static int ClosestPowerOf2Up(int value)
        {
            switch (value)
            {
                case 0:
                    return 0;
                case 1:
                    return 1;
                case 2:
                    return 2;
                case <= 4:
                    return 4;
                case <= 8:
                    return 8;
                case <= 16:
                    return 16;
                case <= 32:
                    return 32;
                case <= 64:
                    return 64;
                case <= 128:
                    return 128;
                case <= 256:
                    return 256;
                case <= 512:
                    return 512;
                case <= 1024:
                    return 1024;
                case <= 2048:
                    return 2048;
                case <= 4096:
                    return 4096;
                case <= 8192:
                    return 8192;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        /// <summary>
        /// Ceils font size to multiplicative of 12
        /// <remarks>
        /// Max value is 120
        /// </remarks>
        /// </summary>
        public static int ClosestFontSizeUp(float size)
        {
            return size switch
            {
                <= 12 => 12,
                <= 24 => 24,
                <= 36 => 36,
                <= 48 => 48,
                <= 60 => 60,
                <= 72 => 72,
                <= 84 => 84,
                <= 96 => 96,
                <= 108 => 108,
                <= 120 => 120,
                _ => throw new ScveException("Font sizes higher than 120 are unsupported")
            };
        }

        /// <summary>
        /// Converts Line Height into FontSize, Opposite to <see cref="FontSizeToLineHeight"/>
        /// </summary>
        /// <remarks>
        /// https://websemantics.uk/tools/font-size-conversion-pixel-point-em-rem-percent/
        /// </remarks>
        public static float LineHeightToFontSize(float lineHeight)
        {
            return lineHeight * 3 / 4;
        }

        /// <summary>
        /// Converts FontSize into Line Height, Opposite to <see cref="LineHeightToFontSize"/>
        /// </summary>
        /// <remarks>
        /// https://websemantics.uk/tools/font-size-conversion-pixel-point-em-rem-percent/
        /// </remarks>
        public static float FontSizeToLineHeight(float fontSize)
        {
            return fontSize * 4 / 3;
        }

        public static bool PointInRect(float x, float y, float width, float height, float px, float py)
        {
            return px >= x && px <= x + width &&
                   py >= y && py <= y + height;
        }
    }
}