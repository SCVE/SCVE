using SCVE.Core.Rendering;

namespace SCVE.Core.Texts
{
    public class FontLoadData
    {
        public FontAtlasData FontAtlasData { get; set; }

        public TextureData AtlasTextureData { get; set; }

        public FontLoadData(FontAtlasData fontAtlasData, TextureData atlasTextureData)
        {
            FontAtlasData = fontAtlasData;
            AtlasTextureData = atlasTextureData;
        }
    }
}