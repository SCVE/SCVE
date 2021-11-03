using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using SCVE.Core.Loading;
using SCVE.Core.Services;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;
using SharpFont;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.ImageSharpBindings
{
    public class SharpFontImageSharpFontAtlasGenerator : IFontAtlasGenerator
    {
        private const string BasePath = "assets/Font";

        public void Generate(string fontFileName, string alphabet, float fontSize)
        {
            var fontName = Path.GetFileNameWithoutExtension(fontFileName);
            var font = new FontFace(File.OpenRead($"{BasePath}/{fontFileName}"));

            // https://websemantics.uk/tools/font-size-conversion-pixel-point-em-rem-percent/
            float lineHeight = Maths.FontSizeToLineHeight(fontSize);
            var faceMetrics = font.GetFaceMetrics(lineHeight);

            int chunkWidth = (int)lineHeight;

            // fit as much characters as possible into a square (+ 1 for space)
            int chunksInARow = (int)MathF.Floor(MathF.Sqrt(alphabet.Length + 1));

            var imageWidth = Maths.ClosestPowerOf2Up(chunksInARow) * Maths.ClosestPowerOf2Up(chunkWidth);

            Image<Rgba32> atlas = new Image<Rgba32>(imageWidth, imageWidth);

            int usedChunks = 0;

            FontAtlasFileData fontAtlasFileData = new FontAtlasFileData(chunkWidth, faceMetrics.CellDescent);

            HashSet<char> atlasChars = new HashSet<char>();

            fontAtlasFileData.Add(
                c: (int)' ',
                new FontAtlasChunk(
                    advance: lineHeight / 2,
                    textureCoordX: 0,
                    textureCoordY: 0,
                    bearingX: 0,
                    bearingY: 0,
                    0
                ));
            usedChunks++;

            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i] == ' ') continue;

                var glyph = font.GetGlyph(alphabet[i], lineHeight);

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
                        textureCoordX: chunkX,
                        textureCoordY: chunkY,
                        bearingX: glyph.HorizontalMetrics.Bearing.X,
                        bearingY: glyph.HorizontalMetrics.Bearing.Y,
                        sizeTextureY: surface.Height
                    ));
            }

            atlas.Save($"{BasePath}/{fontName}/{fontSize}.png");

            var json = JsonSerializer.Serialize(fontAtlasFileData, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

            File.WriteAllText($"{BasePath}/{fontName}/{fontSize}.json", json);
        }

        public unsafe Surface RenderGlyph(Glyph glyph)
        {
            var surface = new Surface
            {
                Bits   = Marshal.AllocHGlobal(glyph.RenderWidth * glyph.RenderHeight),
                Width  = glyph.RenderWidth,
                Height = glyph.RenderHeight,
                Pitch  = glyph.RenderWidth
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

            // TODO: Image allocation is out of scope here. We can extract SharpFont glyph rendering to RGBA byte array and then use the byte array in ImageSharp separately 

            var image = Image.WrapMemory<Rgba32>(Configuration.Default, pixels, width, height);

            Marshal.FreeHGlobal(surface.Bits); //Give the memory back!
            return image;
        }
    }
}