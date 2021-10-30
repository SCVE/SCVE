using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class TextComponent : RenderableComponent
    {
        public ScveFont Font { get; set; }

        private string _fontFileName;
        private float _fontSize;
        private string _text;
        
        
        public TextComponent(string fontFileName, float fontSize, string text)
        {
            Logger.Construct(nameof(TextComponent));
            _fontFileName = fontFileName;
            _fontSize = fontSize;
            _text = text;

            // NOTE: for text alignment we can precalculate the sum of all advances (width of the text)

            Application.Instance.Input.Scroll += InputOnScroll;

            Rebuild();
        }

        protected override void OnResized()
        {
            // We need to override this, because no scaling is needed for text
            var translation = TranslationMatrix.MakeTranslation(X, Y);
            ModelMatrix.MakeIdentity().Multiply(translation);
        }

        private void InputOnScroll(float arg1, float arg2)
        {
            Logger.Warn($"Scrolled {arg2}");
            _fontSize += arg2;
            Rebuild();
        }

        private void Rebuild()
        {
            Font = Application.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));
        }

        public override void Render(IRenderer renderer)
        {
            renderer.RenderText(Font, _text, _fontSize, X, Y, PixelWidth, PixelHeight);
        }
    }
}