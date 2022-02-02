using System;

namespace SCVE.Editor.Imaging
{
    public class ThreeWayImage : IImage
    {
        private readonly string _name;

        public int Width { get; set; }
        public int Height { get; set; }

        private IImage _cpuImage;
        private IImage _gpuImage;
        private IImage _diskImage;

        public ImagePresence Presence => _presence;

        private ImagePresence _presence;

        /// <summary>
        /// Constructs a bald new image with dimensions
        /// </summary>
        public ThreeWayImage(int width, int height, string name)
        {
            _name     = name;
            Width     = width;
            Height    = height;
            _presence = ImagePresence.NO;
        }

        public ThreeWayImage(CpuImage cpuImage, string name)
        {
            _name     = name;
            _cpuImage = cpuImage;
            _presence = ImagePresence.CPU;
        }

        public ThreeWayImage(GpuImage gpuImage, string name)
        {
            _name     = name;
            _gpuImage = gpuImage;
            _presence = ImagePresence.GPU;
        }

        public ThreeWayImage(DiskCachedImage diskImage, string name)
        {
            _name      = name;
            _diskImage = diskImage;
            _presence  = ImagePresence.DISK;
        }

        public void ReplaceContent(ThreeWayImage image)
        {
            if (_presence == ImagePresence.NO)
            {
                _cpuImage  = image._cpuImage;
                _gpuImage  = image._gpuImage;
                _diskImage = image._diskImage;

                Width     = image.Width;
                Height    = image.Height;
                _presence = image._presence;
            }
            else
            {
                throw new ArgumentOutOfRangeException("The image was not fully unloaded.");
            }
        }

        public void ToCpu()
        {
            switch (_presence)
            {
                case ImagePresence.NO:
                    // Materialize a new image
                    _cpuImage = new CpuImage(Width, Height);
                    break;
                case ImagePresence.CPU:
                    // NOOP
                    break;
                case ImagePresence.GPU:
                    _cpuImage = new CpuImage(_gpuImage);
                    _gpuImage.Dispose();
                    _gpuImage = null;
                    break;
                case ImagePresence.DISK:
                    _cpuImage = new CpuImage(_diskImage);
                    _diskImage.Dispose();
                    _diskImage = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Image instance has an unknown ImagePresence value.");
            }

            _presence = ImagePresence.CPU;
        }

        public void ToGpu()
        {
            switch (_presence)
            {
                case ImagePresence.NO:
                    throw new ArgumentOutOfRangeException("Image instance has a ImagePresence.NO value and can't be uploaded to gpu");
                case ImagePresence.CPU:
                    _gpuImage = new GpuImage(_cpuImage);
                    _cpuImage.Dispose();
                    _cpuImage = null;
                    break;
                case ImagePresence.GPU:
                    // NOOP
                    break;
                case ImagePresence.DISK:
                    _gpuImage = new GpuImage(_diskImage);
                    _diskImage.Dispose();
                    _diskImage = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Image instance has an unknown ImagePresence value.");
            }

            _presence = ImagePresence.GPU;
        }

        public void ToDisk()
        {
            switch (_presence)
            {
                case ImagePresence.NO:
                    throw new ArgumentOutOfRangeException("Image instance has a ImagePresence.NO value and can't be uploaded to disk");
                case ImagePresence.CPU:
                    _diskImage = new GpuImage(_cpuImage);
                    _cpuImage.Dispose();
                    _cpuImage = null;
                    break;
                case ImagePresence.GPU:
                    _diskImage = new DiskCachedImage(_gpuImage, $"cached/{_name}.uncompressed");
                    _gpuImage.Dispose();
                    _gpuImage = null;
                    break;
                case ImagePresence.DISK:
                    // NOOP
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Image instance has an unknown ImagePresence value.");
            }

            _presence = ImagePresence.DISK;
        }

        public void ToNo()
        {
            switch (_presence)
            {
                case ImagePresence.NO:
                    break;
                case ImagePresence.CPU:
                    _cpuImage.Dispose();
                    _cpuImage = null;
                    break;
                case ImagePresence.GPU:
                    _gpuImage.Dispose();
                    _gpuImage = null;
                    break;
                case ImagePresence.DISK:
                    _diskImage.Dispose();
                    _diskImage = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Image instance has an unknown ImagePresence value.");
            }

            _presence = ImagePresence.NO;
        }

        public byte[] ToByteArray()
        {
            return _presence switch
            {
                ImagePresence.CPU => _cpuImage.ToByteArray(),
                ImagePresence.GPU => _gpuImage.ToByteArray(),
                ImagePresence.DISK => _diskImage.ToByteArray(),
                ImagePresence.NO => throw new ArgumentOutOfRangeException("Image instance has a ImagePresence.NO value and can't be materialized"),
                _ => throw new ArgumentOutOfRangeException("Image instance has an unknown ImagePresence value.")
            };
        }

        public void Dispose()
        {
            _cpuImage?.Dispose();
            _gpuImage?.Dispose();
            _diskImage?.Dispose();
        }
    }
}