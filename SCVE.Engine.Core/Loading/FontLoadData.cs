namespace SCVE.Engine.Core.Loading
{
    public class FontLoadData
    {
        public FontAtlasFileData FontAtlasFileData { get; set; }

        public TextureFileData AtlasTextureFileData { get; set; }

        public FontLoadData(FontAtlasFileData fontAtlasFileData, TextureFileData atlasTextureFileData)
        {
            FontAtlasFileData = fontAtlasFileData;
            AtlasTextureFileData = atlasTextureFileData;
        }
    }
}