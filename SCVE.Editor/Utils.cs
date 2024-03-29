﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
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

        public static void ShuffleRgba32ToBgr32(byte[] pixels)
        {
            if (false && Avx2.IsSupported)
            {
                const byte byte0 = 2 + 0;
                var mask = Vector128.Create(
                    byte0, 1 + 0, 0 + 0, 3 + 0,
                    2 + 4, 1 + 4, 0 + 4, 3 + 4,
                    2 + 8, 1 + 8, 0 + 8, 3 + 8,
                    2 + 12, 1 + 12, 0 + 12, 3 + 12);

                for (var i = 0; i < pixels.Length - 16; i += 16)
                {
                    Vector128<byte> vector = Vector128.Create(
                        pixels[0],
                        pixels[1],
                        pixels[2],
                        pixels[3],
                        pixels[4],
                        pixels[5],
                        pixels[6],
                        pixels[7],
                        pixels[8],
                        pixels[9],
                        pixels[10],
                        pixels[11],
                        pixels[12],
                        pixels[13],
                        pixels[14],
                        pixels[15]
                    );

                    var result = Avx2.Shuffle(vector, mask);

                    var asVector = Vector128.AsVector(result);

                    asVector.CopyTo(pixels, i);
                }

                for (int i = pixels.Length - 16; i < pixels.Length; i += 4)
                {
                    byte r = pixels[i + 0];
                    byte g = pixels[i + 1];
                    byte b = pixels[i + 2];
                    byte a = pixels[i + 3];

                    pixels[i + 0] = b;
                    pixels[i + 1] = g;
                    pixels[i + 2] = r;
                    pixels[i + 3] = a;
                }
            }
            else
            {
                for (var i = 0; i < pixels.Length; i += 4)
                {
                    byte r = pixels[i + 0];
                    byte g = pixels[i + 1];
                    byte b = pixels[i + 2];
                    byte a = pixels[i + 3];

                    pixels[i + 0] = b;
                    pixels[i + 1] = g;
                    pixels[i + 2] = r;
                    pixels[i + 3] = a;
                }
            }
        }
    }
}