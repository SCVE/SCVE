using System;
using SCVE.Core.App;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class TextComponent : RenderableComponent
    {
        public ScveFont Font { get; set; }

        private string _fontFileName;
        private float _fontSize;
        private string _text;

        private string[] _lines;
        private float[] _lineWidths;

        private TextAlignment _alignment;

        public TextComponent(string fontFileName, float fontSize, string text, TextAlignment alignment)
        {
            Logger.Construct(nameof(TextComponent));
            _fontFileName = fontFileName;
            _fontSize = fontSize;
            _text = text;
            _alignment = alignment;

            // NOTE: for text alignment we can precalculate the sum of all advances (width of the text)

            Application.Instance.Input.Scroll += InputOnScroll;

            Font = Application.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));
            
            Rebuild();
        }

        protected override void OnResized()
        {
            // We need to override this, because no scaling or translatin is needed for text
        }

        private void InputOnScroll(float arg1, float arg2)
        {
            Logger.Warn($"Scrolled {arg2}");
            _fontSize += arg2;
            Font = Application.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));
            Rebuild();
        }

        private void Rebuild()
        {
            if (_alignment == TextAlignment.Left)
            {
                // Don't even calculate or allocate memory, when text is aligned to left, it's automatically rendered as a single scve-render pass
                return;
            }
            
            _lines = _text.Split('\n');
            _lineWidths = new float[_lines.Length];
            for (var i = 0; i < _lines.Length; i++)
            {
                var textMeasurement = TextMeasurer.MeasureText(Font, _lines[i], _fontSize);
                _lineWidths[i] = textMeasurement.Width;
            }
        }

        public override void Render(IRenderer renderer)
        {
            var lineHeight = Maths.FontSizeToLineHeight(_fontSize);
            switch (_alignment)
            {
                case TextAlignment.Left:
                {
                    // When rendering with left alignment, renderer will take care of line alignment (it's always zero)
                    renderer.RenderText(Font, _text, _fontSize, X, Y, PixelWidth, PixelHeight);
                    break;
                }
                case TextAlignment.Center:
                {
                    // When rendering with center alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, X + PixelWidth / 2 - _lineWidths[i] / 2, Y + lineHeight * i);
                    }
                    break;
                }
                case TextAlignment.Right:
                {
                    // When rendering with right alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, X + PixelWidth - _lineWidths[i], Y + lineHeight * i);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_alignment));
            }
        }
    }
}