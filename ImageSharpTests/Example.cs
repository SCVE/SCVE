using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using SCVE.Core.Loading;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;
using SharpFont;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpTests
{
    public class Example
    {
        public void Run()
        {
            float fontSize = 32;

            var font = new FontFace(File.OpenRead("assets/Fonts/arial.ttf"));

            string alphabet =
                "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" +
                "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                "0123456789" +
                "!\"#$%^&*()+=-_'?.,|/`~№:;@[]{}";

            int chunkWidth = (int)fontSize;

            // fit as much characters as possible into a square
            int chunksInARow = (int)MathF.Floor(MathF.Sqrt(alphabet.Length));

            var imageWidth = Maths.ClosestPowerOf2Up(chunksInARow) * Maths.ClosestPowerOf2Up(chunkWidth);

            Image<Rgba32> atlas = new Image<Rgba32>(imageWidth, imageWidth);

            int usedChunks = 0;

            FontAtlasFileData fontAtlasFileData = new FontAtlasFileData(chunkWidth);

            HashSet<char> atlasChars = new HashSet<char>();

            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i] == ' ') continue;

                var glyph = font.GetGlyph(alphabet[i], fontSize);

                if (alphabet[i] == 'g')
                {
                    int a = 5;
                }

                var surface = RenderGlyph(glyph);

                var image = ToImage(surface);

                if (!atlasChars.Add(alphabet[i]))
                    continue;

                var chunkRowIndex = (usedChunks % chunksInARow);
                var chunkColumnIndex = (usedChunks / chunksInARow);

                int chunkX = chunkWidth * chunkRowIndex;
                int chunkY = chunkWidth * chunkColumnIndex;
                usedChunks++;

                atlas.Mutate(a => a.DrawImage(image, new Point(chunkX, chunkY), 1f));

                fontAtlasFileData.Add(
                    c: (int)alphabet[i],
                    new FontAtlasChunk(
                        advance: glyph.HorizontalMetrics.Advance,
                        textureX: chunkX,
                        textureY: chunkY,
                        bearingX: glyph.HorizontalMetrics.Bearing.X,
                        bearingY: glyph.HorizontalMetrics.Bearing.Y
                    ));
            }

            atlas.Save($"assets/Fonts/arial/atlas.png");

            var json = JsonSerializer.Serialize(fontAtlasFileData, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            File.WriteAllText($"assets/Fonts/arial/atlasData.json", json);
        }

        public unsafe Surface RenderGlyph(Glyph glyph)
        {
            var surface = new Surface
            {
                Bits = Marshal.AllocHGlobal(glyph.RenderWidth * glyph.RenderHeight),
                Width = glyph.RenderWidth,
                Height = glyph.RenderHeight,
                Pitch = glyph.RenderWidth
            };

            var stuff = (byte*)surface.Bits;
            for (int i = 0; i < surface.Width * surface.Height; i++)
                *stuff++ = 0;

            glyph.RenderTo(surface);

            return surface;
        }

        private Image<Rgba32> ToImage(Surface surface)
        {
            int width = surface.Width;
            int height = surface.Height;
            int len = width * height;
            byte[] data = new byte[len];
            Marshal.Copy(surface.Bits, data, 0, len);
            byte[] pixels = new byte[len * 4];

            int index = 0;
            for (int i = 0; i < len; i++)
            {
                byte c = data[i];
                pixels[index++] = byte.MaxValue;
                pixels[index++] = byte.MaxValue;
                pixels[index++] = byte.MaxValue;
                pixels[index++] = c;
            }

            var image = Image.WrapMemory<Rgba32>(Configuration.Default, pixels, width, height);

            Marshal.FreeHGlobal(surface.Bits); //Give the memory back!
            return image;
        }
    }
}