using System.Collections.Generic;
using SCVE.Engine.Core.Loading;

namespace SCVE.Engine.Core.Texts
{
    public class FontAtlas
    {
        /// <summary>
        /// PixelWidth of each character in a Font Atlas
        /// </summary>
        public int ChunkSize { get; set; }

        public float CellDescent { get; set; }
        
        // TODO: LineHeight
        
        /// <summary>
        /// All TextureAtlas Chunks, indexed by their int valued
        /// </summary>
        public Dictionary<int, FontAtlasChunk> Chunks { get; set; }

        public FontAtlas(FontAtlasFileData fontAtlasFileData)
        {
            ChunkSize = fontAtlasFileData.ChunkSize;
            Chunks = fontAtlasFileData.Chunks;
            CellDescent = fontAtlasFileData.CellDescent;
        }
        
        public FontAtlas(int chunkSize, Dictionary<int, FontAtlasChunk> chunks, float cellDescent)
        {
            ChunkSize = chunkSize;
            Chunks = chunks;
            CellDescent = cellDescent;
        }
    }
}