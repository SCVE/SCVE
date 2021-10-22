using System.Collections.Generic;

namespace SCVE.Core.Texts
{
    public class FontAtlasData
    {
        public int ChunkWidth { get; set; }
        
        public Dictionary<int, FontAtlasChunk> Chunks { get; set; }

        public FontAtlasData(int chunkWidth)
        {
            ChunkWidth = chunkWidth;
            Chunks = new Dictionary<int, FontAtlasChunk>();
        }

        public void Add(int c, FontAtlasChunk data)
        {
            Chunks.Add(c, data);
        }
    }
}