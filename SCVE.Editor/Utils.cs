using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using SCVE.Editor.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using FontCollection = SixLabors.Fonts.FontCollection;

namespace SCVE.Editor
{
    public static class Utils
    {
        public static bool IsDirectory(this FileSystemInfo info)
        {
            // get the file attributes for file or directory
            FileAttributes attr = info.Attributes;

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            else
                return false;
        }

        public static bool IsDirectoryPath(this string path)
        {
            return Directory.Exists(path);
        }

        public static IEnumerable<Type> GetAssignableTypesFromAssembly<T>(Assembly assembly)
        {
            return assembly.ExportedTypes
                .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface);
        }

        public static IList<Type> GetAssignableTypes<T>()
        {
            var referencedAssemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            var types = referencedAssemblyNames.SelectMany(name =>
                    GetAssignableTypesFromAssembly<T>(Assembly.Load(name))
                )
                .Concat(
                    GetAssignableTypesFromAssembly<T>(Assembly.GetExecutingAssembly())
                )
                .ToList();

            return types;
        }

        public static ThreeWayImage CreateNoPreviewImage(int width, int height)
        {
            var previewImage = new CpuImage(new byte[width * height * 4], width, height);

            using var image = Image.WrapMemory<Rgba32>(previewImage.ToByteArray(), width, height);

            FontCollection fontCollection = new FontCollection();
            fontCollection.Add("assets/Font/arial.ttf");
            var font = fontCollection.Get("arial").CreateFont(72);
            image.Mutate(i => i.DrawText($"NO PREVIEW", font, Color.Red, new PointF(10, 0)));

            return new ThreeWayImage(previewImage, "NO PREVIEW");
        }

        /// <summary>
        /// Reads JSON file contents into an object, throws on any invalid data, so be careful 
        /// </summary>
        public static T ReadJson<T>(string path)
        {
            var jsonContent = File.ReadAllText(path);

            var obj = JsonSerializer.Deserialize<T>(jsonContent,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return obj;
        }

        /// <summary>
        /// Writes object to a JSON file, throws on any invalid data, so be careful 
        /// </summary>
        public static void WriteJson<T>(T obj, string path)
        {
            var jsonContent = JsonSerializer.Serialize(obj,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                });

            File.WriteAllText(path, jsonContent);
        }
    }
}