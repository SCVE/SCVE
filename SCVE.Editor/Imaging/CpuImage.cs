using System;

namespace SCVE.Editor.Imaging
{
    public class CpuImage : IImage
    {
        private byte[] _bytes;

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// Constructs new empty RGBA image in memory
        /// </summary>
        public CpuImage(int width, int height)
        {
            Width  = width;
            Height = height;

            _bytes = new byte[Width * Height * 4];
        }

        /// <summary>
        /// Copies an image into memory independently of source location
        /// </summary>
        public CpuImage(IImage image)
        {
            Width  = image.Width;
            Height = image.Height;

            _bytes = new byte[Width * Height * 4];

            // copy the underlying memory
            Buffer.BlockCopy(image.ToByteArray(), 0, _bytes, 0, _bytes.Length);
        }

        /// <summary>
        /// Wraps a RGBA image from a byte array
        /// </summary>
        public CpuImage(byte[] bytes, int width, int height)
        {
            Width  = width;
            Height = height;

            _bytes = bytes;
        }

        public byte[] ToByteArray()
        {
            return _bytes;
        }

        public void Dispose()
        {
        }
    }
}