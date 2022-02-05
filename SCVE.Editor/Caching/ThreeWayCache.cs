using System.Collections.Generic;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Caching
{
    public class ThreeWayCache
    {
        public IReadOnlyList<ThreeWayImage> Images => _images;

        private List<ThreeWayImage> _images;

        public int Width { get; set; }
        public int Height { get; set; }

        public ThreeWayCache(int length, int width, int height)
        {
            _images = new List<ThreeWayImage>();
            Width   = width;
            Height  = height;
            Resize(length);
        }

        public void Resize(int newSize)
        {
            while (_images.Count > newSize)
            {
                _images[^1].Dispose();
                _images.RemoveAt(_images.Count - 1);
            }

            _images.Capacity = newSize;
            while (_images.Count < newSize)
            {
                _images.Add(new ThreeWayImage(Width, Height, _images.Count.ToString()));
            }
        }

        public void Put(int index, ThreeWayImage image)
        {
            _images[index].Dispose();
            _images[index] = image;
        }

        public bool HasAnyPresence(int index)
        {
            return _images[index].Presence != ImagePresence.NO;
        }

        public bool TryMakeFromDisk(int index)
        {
            return _images[index].TryMakeFromDisk();
        }

        public void Invalidate(int index)
        {
            _images[index].ToNo();
        }

        public ThreeWayImage this[int index] => _images[index];
    }
}