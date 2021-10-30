using System;
using System.IO;
using System.Text.Json;
using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Loading;
using SCVE.Core.Loading.Loaders;
using SCVE.Core.Services;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;

namespace SCVE.Platform.Windows
{
    public class WindowsFontFileLoader : IFontLoader
    {
        private static readonly string BaseDirectory = $"assets/Font";
        
        public FontLoadData Load(string fontFileName, float lineHeight)
        {
            if (File.Exists($"{BaseDirectory}/{fontFileName}"))
            {
                var fontSize = Maths.LineHeightToFontSize(lineHeight);
                var fontName = Path.GetFileNameWithoutExtension(fontFileName);

                if (!Directory.Exists($"{BaseDirectory}/{fontName}"))
                {
                    Directory.CreateDirectory($"{BaseDirectory}/{fontName}");
                }
                
                var fontAtlasFileDataPath = $"{BaseDirectory}/{fontName}/{fontSize}.json";
                if (!File.Exists(fontAtlasFileDataPath))
                {
                    // No atlas, so we need to create one
                    Application.Instance.FontAtlasGenerator.Generate(fontFileName, Alphabets.Default, fontSize);
                }
            
                string json = File.ReadAllText(fontAtlasFileDataPath);
                var fontAtlasFileData = JsonSerializer.Deserialize<FontAtlasFileData>(json);
            
                var fontAtlasFileTexturePath = $"{BaseDirectory}/{fontName}/{fontSize}.png";

                var textureFileData = Application.Instance.FileLoaders.Texture.Load(fontAtlasFileTexturePath);

                return new FontLoadData(fontAtlasFileData, textureFileData);
            }
            else
            {
                // Font even not exists in BaseDirectory
                // TODO: Fallback to default

                var defaultFontLoad = Load("default.ttf", lineHeight);

                if (defaultFontLoad == null)
                {
                    throw new ScveException("Default font (default.ttf) was not found");
                }

                return defaultFontLoad;
            }
        }
    }
}