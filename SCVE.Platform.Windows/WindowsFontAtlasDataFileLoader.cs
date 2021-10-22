using System.IO;
using System.Text.Json;
using SCVE.Core.Loading;
using SCVE.Core.Services;
using SCVE.Core.Texts;

namespace SCVE.Platform.Windows
{
    public class WindowsFontAtlasDataFileLoader : FileLoader<FontAtlasData>
    {
        private static readonly string BaseDirectory = $"assets/Font";
        
        public override FontAtlasData Load(string fileName)
        {
            var fontName = Path.GetFileNameWithoutExtension(fileName);
            string json = File.ReadAllText($"{BaseDirectory}/{fontName}/atlasData.json");
            return JsonSerializer.Deserialize<FontAtlasData>(json);
        }
    }
}