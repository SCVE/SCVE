namespace SCVE.Core.Loading.Loaders
{
    public interface IFontLoader
    {
        FontLoadData Load(string fontFileName, float size);
    }
}