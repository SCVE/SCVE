using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using JsonSubTypes;
using Newtonsoft.Json;
using SCVE.Editor.Editing.Effects;
using SCVE.Editor.Imaging;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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

        public static void ClearContent(this DirectoryInfo directoryInfo)
        {
            foreach (var directory in directoryInfo.EnumerateDirectories())
            {
                directory.Delete(true);
            }

            foreach (var file in directoryInfo.EnumerateFiles())
            {
                file.Delete();
            }
        }

        public static IEnumerable<Type> GetAssignableTypesFromAssembly<T>(Assembly assembly)
        {
            return assembly.ExportedTypes
                .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface);
        }

        public static IList<Type> GetAssignableTypes<T>()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblyNames = executingAssembly.GetReferencedAssemblies();
            var types = referencedAssemblyNames.SelectMany(name =>
                    GetAssignableTypesFromAssembly<T>(Assembly.Load(name))
                )
                .Concat(
                    GetAssignableTypesFromAssembly<T>(executingAssembly)
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

        private static JsonSerializerSettings _settings;

        private static JsonSerializerSettings GetJsonSettings()
        {
            if (_settings is not null)
            {
                return _settings;
            }

            var settings = new JsonSerializerSettings();

            settings.Formatting = Formatting.Indented;

            var allEffectTypes = GetAssignableTypes<EffectBase>();
            var effectsSubtypesBuilder = JsonSubtypesConverterBuilder
                .Of<EffectBase>("Type");
            foreach (var effectType in allEffectTypes)
            {
                effectsSubtypesBuilder.RegisterSubtype(effectType, effectType.Name);
            }

            effectsSubtypesBuilder.SerializeDiscriminatorProperty(true);

            settings.Converters.Add(effectsSubtypesBuilder.Build());

            return _settings = settings;
        }

        /// <summary>
        /// Reads JSON file contents into an object, throws on any invalid data, so be careful 
        /// </summary>
        public static T ReadJson<T>(string path)
        {
            var jsonContent = File.ReadAllText(path);

            var obj = JsonConvert.DeserializeObject<T>(jsonContent, GetJsonSettings());

            return obj;
        }

        /// <summary>
        /// Writes object to a JSON file, throws on any invalid data, so be careful 
        /// </summary>
        public static void WriteJson<T>(T obj, string path)
        {
            var jsonContent = JsonConvert.SerializeObject(obj, GetJsonSettings());

            File.WriteAllText(path, jsonContent);
        }

        /// <summary>
        /// Fits a rect inside another rect
        /// <remarks>
        /// https://stackoverflow.com/a/21960701
        /// </remarks>
        /// </summary>
        public static Vector2 FitRect(Vector2 container, Vector2 subject)
        {
            float innerAspectRatio = subject.X / subject.Y;
            float outerAspectRatio = container.X / container.Y;

            float resizeFactor = (innerAspectRatio >= outerAspectRatio) ? (container.X / subject.X) : (container.Y / subject.Y);

            float newWidth = subject.X * resizeFactor;
            float newHeight = subject.Y * resizeFactor;

            return new Vector2(newWidth, newHeight);
        }
    }
}