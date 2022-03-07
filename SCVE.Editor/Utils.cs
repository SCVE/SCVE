using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Imaging;
using SCVE.Editor.ImGuiUi;
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
    }
}