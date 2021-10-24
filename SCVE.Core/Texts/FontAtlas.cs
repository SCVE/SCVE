using System.Collections.Generic;
using SCVE.Core.Loading;

namespace SCVE.Core.Texts
{
    public class FontAtlas
    {
        /// <summary>
        /// PixelWidth of each character in a Font Atlas
        /// </summary>
        public int ChunkSize { get; set; }
        
        // TODO: LineHeight
        
        /// <summary>
        /// All TextureAtlas Chunks, indexed by their int valued
        /// </summary>
        public Dictionary<int, FontAtlasChunk> Chunks { get; set; }

        public FontAtlas(FontAtlasFileData fontAtlasFileData)
        {
            ChunkSize = fontAtlasFileData.ChunkSize;
            Chunks = fontAtlasFileData.Chunks;
        }
        
        public FontAtlas(int chunkSize, Dictionary<int, FontAtlasChunk> chunks)
        {
            ChunkSize = chunkSize;
            Chunks = chunks;
        }
    }
}