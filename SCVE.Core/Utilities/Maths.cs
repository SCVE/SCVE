using System;

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
    }
}