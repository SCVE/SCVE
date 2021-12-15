namespace SCVE.Engine.Core.Loading.Loaders
{
    public interface ITextureLoader
    {
        /// <summary>
        /// Returns an array of pixels in RGBA format
        /// </summary>
        TextureFileData Load(string path, bool flip = true);
    }
}