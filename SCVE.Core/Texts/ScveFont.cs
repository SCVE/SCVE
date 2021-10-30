using System.Collections.Generic;
using SCVE.Core.App;
using SCVE.Core.Loading;
using SCVE.Core.Rendering;

namespace SCVE.Core.Texts
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
            Texture = Application.Instance.RenderEntitiesCreator.CreateTexture(loadData.AtlasTextureFileData);
        }
    }
}