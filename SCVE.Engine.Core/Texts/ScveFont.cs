using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Loading;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.Core.Texts
{
    public class ScveFont
    {
        /// <summary>
        /// The height of this font
        /// </summary>
        public float LineHeight { get; set; }

        public FontAtlas Atlas { get; set; }

        /// <summary>
        /// Texture, holding the atlas
        /// </summary>
        public Texture Texture { get; set; }

        public ScveFont(FontLoadData loadData, float lineHeight)
        {
            LineHeight = lineHeight;
            Atlas = new FontAtlas(loadData.FontAtlasFileData);
            Texture = Main.ScveEngine.Instance.RenderEntitiesCreator.CreateTexture(loadData.AtlasTextureFileData);
        }
    }
}