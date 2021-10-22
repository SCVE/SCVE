using SCVE.Core.Loading;

namespace SCVE.Core.Rendering
{
    public interface ITextureLoader
    {
        /// <summary>
        /// Returns an array of pixels in RGBA format
        /// </summary>
        TextureData Load(string path);
    }
}