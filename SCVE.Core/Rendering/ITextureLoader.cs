namespace SCVE.Core.Rendering
{
    public interface ITextureLoader
    {
        /// <summary>
        /// Returns an array of pixels in RGBA format
        /// </summary>
        byte[] Load(string path);
    }
}