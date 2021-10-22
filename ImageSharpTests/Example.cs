using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using SCVE.Core.Utilities;
using SharpFont;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageSharpTests
{
    public class FontAtlasChunk
    {
        /// <summary>
        /// Linear advance of the char
        /// </summary>
        public float Advance { get; set; }

        /// <summary>
        /// Texture Coordinate X of a chunk
        /// </summary>
        public int TextureX { get; set; }

        /// <summary>
        /// Texture Coordinate Y of a chunk
        /// </summary>
        public int TextureY { get; set; }

        /// <summary>
        /// Offset from left of the chunk
        /// </summary>
        public float BearingX { get; set; }

        /// <summary>
        /// Offset from top of the chunk
        /// </summary>
        public float BearingY { get; set; }

        public FontAtlasChunk(float advance, int textureX, int textureY, float bearingX, float bearingY)
        {
            Advance = advance;
            TextureX = textureX;
            TextureY = textureY;
            BearingX = bearingX;
            BearingY = bearingY;
        }
    }

    public class FontAtlasData
    {
        public Dictionary<int, FontAtlasChunk> Chunks { get; set; }

        public FontAtlasData()
        {
            Chunks = new Dictionary<int, FontAtlasChunk>();
        }

        public void Add(int c, FontAtlasChunk data)
        {
            Chunks.Add(c, data);
        }
    }

    public class Example
    {
        public void Run()
        {
            var font = new FontFace(File.OpenRead("assets/Fonts/arial.ttf"));

            string alphabet =
                "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "абвгдеёжзийклмнопрстуфхцчшщъыьэюя" +
                "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                "0123456789" +
                "!\"#$%^&*()+=-_'?.,|/`~№:;@[]{}";

            int chunkWidth = 32;

            // fit as much characters as possible into a square
            int chunksInARow = (int)MathF.Floor(MathF.Sqrt(alphabet.Length));

            var imageWidth = Maths.ClosestPowerOf2Up(chunksInARow) * Maths.ClosestPowerOf2Up(chunkWidth);

            Image<Rgba32> atlas = new Image<Rgba32>(imageWidth, imageWidth);

            int usedChunks = 0;

            FontAtlasData fontAtlasData = new FontAtlasData();

            HashSet<char> atlasChars = new HashSet<char>();

            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i] == ' ') continue;

                var glyph = font.GetGlyph(alphabet[i], 32);

                if (alphabet[i] == 'g')
                {
                    int a = 5;
                }

                var surface = RenderGlyph(glyph);

                var image = ToImage(surface);

                // image.Save($"assets/Fonts/arial/" +
                //            $"{(int)word[i]:0000}_" +
                //            $"{(int)glyph.Width}_" +
                //            $"{(int)glyph.Height}_" +
                //            $"{glyph.HorizontalMetrics.Advance.ToString("F1").Replace(',', '.')}" +
                //            $".png");

                if (!atlasChars.Add(alphabet[i]))
                    continue;

                var chunkRowIndex = (usedChunks % chunksInARow);
                var chunkColumnIndex = (usedChunks / chunksInARow);

                int chunkX = chunkWidth * chunkRowIndex;
                int chunkY = chunkWidth * chunkColumnIndex;
                usedChunks++;

                atlas.Mutate(a => a.DrawImage(image, new Point(chunkX, chunkY), 1f));

                fontAtlasData.Add(
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

            var json = JsonSerializer.Serialize(fontAtlasData, new JsonSerializerOptions()
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