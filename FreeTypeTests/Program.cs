using SharpFont;

namespace FreeTypeTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // NOTE: We need to copy a pre-built freetype6.dll inside the build folder 
            Library library = new Library();
            Face face = new Face(library, "assets/Font/arial.ttf");
            face.SetPixelSizes(0, 48);
            face.LoadChar('a', LoadFlags.Render, LoadTarget.Normal);

            var faceGlyph = face.Glyph;
            int a = 5;
        }
    }
}