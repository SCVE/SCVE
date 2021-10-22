using System.Collections.Generic;

namespace SCVE.Core.Loading
{
    public class FontAtlasData
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

        public FontAtlasData(int chunkSize)
        {
            ChunkSize = chunkSize;
            Chunks = new Dictionary<int, FontAtlasChunk>();
        }

        public void Add(int c, FontAtlasChunk data)
        {
            Chunks.Add(c, data);
        }
    }
}