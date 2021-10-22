namespace SCVE.Core.Services
{
    public interface IFontAtlasGenerator
    {
        /// <summary>
        /// fontName should contain the extension (ex. arial.ttf)
        /// </summary>
        void Generate(string fontFileName, string alphabet, float fontSize);
    }
}