using System.IO;

namespace SCVE.Editor.Imaging
{
    public class DiskImage : IImage
    {
        /// <summary>
        /// Path, where the image is stored
        /// </summary>
        public string Path { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        /// <summary>
        /// Creates and stores an image into a path
        /// </summary>
        public DiskImage(IImage image, string path)
        {
            Path   = path;
            Width  = image.Width;
            Height = image.Height;
            File.WriteAllBytes(path, image.ToByteArray());
        }

        /// <summary>
        /// Creates an empty on disk image, but doesn't download or upload any data
        /// </summary>
        public DiskImage(int width, int height, string path)
        {
            Path   = path;
            Width  = width;
            Height = height;
        }

        public bool Exists()
        {
            return File.Exists(Path);
        }

        public byte[] ToByteArray()
        {
            return File.ReadAllBytes(Path);
        }

        public void Dispose()
        {
        }
    }
}