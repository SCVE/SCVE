namespace SCVE.Core.Loading
{
    public class FontAtlasChunk
    {
        /// <summary>
        /// Linear advance of the char
        /// </summary>
        public float Advance { get; set; }

        /// <summary>
        /// Texture Coordinate X of a chunk
        /// </summary>
        public int TextureX { get; set; }

        /// <summary>
        /// Texture Coordinate Y of a chunk
        /// </summary>
        public int TextureY { get; set; }

        /// <summary>
        /// Offset from the baseline (X should be always zero)
        /// </summary>
        public float BearingX { get; set; }

        /// <summary>
        /// Offset from the baseline
        /// </summary>
        public float BearingY { get; set; }

        public FontAtlasChunk(float advance, int textureX, int textureY, float bearingX, float bearingY)
        {
            Advance = advance;
            TextureX = textureX;
            TextureY = textureY;
            BearingX = bearingX;
            BearingY = bearingY;
        }
    }
}