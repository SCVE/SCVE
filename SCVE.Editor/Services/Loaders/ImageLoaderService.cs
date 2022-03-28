using System;
using System.IO;
using System.Linq;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services.Loaders
{
    public class ImageLoaderService : IService
    {
        private static string[] _supportedExtensions = {".jpeg", ".jpg", ".png"};

        public static bool IsSupportedExtension(string extension)
        {
            if (extension is null)
            {
                return false;
            }

            return _supportedExtensions.Contains(extension);
        }

        public bool TryLoad(string path, out Image image)
        {
            var extension = Path.GetExtension(path);
            if (_supportedExtensions.Contains(extension))
            {
                var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, path);
                image = Image.CreateNew(relativePath);
                return true;
            }
            else
            {
                Console.WriteLine($"Unknown image type selected: {extension}");

                image = null;
                return false;
            }
        }

        public Image Load(string path)
        {
            var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, path);
            return Image.CreateNew(relativePath);
        }
    }
}