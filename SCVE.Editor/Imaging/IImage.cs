using System;

namespace SCVE.Editor.Imaging
{
    public interface IImage : IDisposable
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] ToByteArray();
    }
}