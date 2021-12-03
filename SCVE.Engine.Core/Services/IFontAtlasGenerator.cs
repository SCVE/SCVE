namespace SCVE.Engine.Core.Services
{
    public interface IFontAtlasGenerator
    {
        /// <summary>
        /// fontName must contain the extension (ex. arial.ttf)
        /// </summary>
        void Generate(string fontFileName, string alphabet, float fontSize);
    }
}