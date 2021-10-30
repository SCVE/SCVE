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
        public int TextureCoordX { get; set; }

        /// <summary>
        /// Texture Coordinate Y of a chunk
        /// </summary>
        public int TextureCoordY { get; set; }

        /// <summary>
        /// Offset from the baseline (X should be always zero)
        /// </summary>
        public float BearingX { get; set; }

        /// <summary>
        /// Offset from the baseline
        /// </summary>
        public float BearingY { get; set; }

        /// <summary>
        /// Real size of the char in a texture
        /// </summary>
        public float SizeTextureY { get; set; }

        public FontAtlasChunk(float advance, int textureCoordX, int textureCoordY, float bearingX, float bearingY, float sizeTextureY)
        {
            Advance = advance;
            TextureCoordX = textureCoordX;
            TextureCoordY = textureCoordY;
            BearingX = bearingX;
            BearingY = bearingY;
            SizeTextureY = sizeTextureY;
        }
    }
}