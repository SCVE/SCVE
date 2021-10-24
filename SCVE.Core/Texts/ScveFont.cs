using System.Collections.Generic;
using SCVE.Core.App;
using SCVE.Core.Loading;
using SCVE.Core.Rendering;

namespace SCVE.Core.Texts
{
    public class ScveFont
    {
        /// <summary>
        /// The size of this font
        /// </summary>
        public float Size { get; set; }

        public FontAtlas Atlas { get; set; }
        
        /// <summary>
        /// Texture, holding the atlas
        /// </summary>
        public Texture Texture { get; set; }

        public ScveFont(FontLoadData loadData, float size)
        {
            Size = size;
            Atlas = new FontAtlas(loadData.FontAtlasFileData);
            Texture = Application.Instance.RenderEntitiesCreator.CreateTexture(loadData.AtlasTextureFileData);
        }
    }
}