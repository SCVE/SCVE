using Silk.NET.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SCVE.Editor.Effects
{
    public class ImageFrame
    {
        public Texture GpuTexture { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public byte[] RawBytes { get; private set; }

        public Image<Rgba32> ImageSharpImage { get; private set; }

        public ImageFrame(int width, int height)
        {
            Width    = width;
            Height   = height;
            RawBytes = new byte[width * height * 4];
        }

        public void CreateImageSharpWrapper()
        {
            ImageSharpImage = Image.WrapMemory<Rgba32>(RawBytes, Width, Height);
        }

        public void Dispose()
        {
            if (ImageSharpImage is not null)
            {
                ImageSharpImage.Dispose();
            }

            if (GpuTexture is not null)
            {
                GpuTexture.Dispose();
            }
        }

        private void UploadToGpu()
        {
            GpuTexture = new Texture(
                gl: EditorApp.Instance.GL,
                width: Width,
                height: Height,
                data: RawBytes,
                pixelFormat: PixelFormat.Rgba
            );
        }

        public void UploadGpuData()
        {
            if (GpuTexture is null)
            {
                UploadToGpu();
            }
            else
            {
                GpuTexture.UpdateData(RawBytes, PixelFormat.Rgba);
            }
        }
    }
}