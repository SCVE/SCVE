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

        public TextComponent(ComponentStyle style, string fontFileName, float fontSize, string text, TextAlignment alignment) : base(style)
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
            float maxLineWidth = 0f;
            for (var i = 0; i < _lines.Length; i++)
            {
                var textMeasurement = TextMeasurer.MeasureText(Font, _lines[i], _fontSize);
                _lineWidths[i] = textMeasurement.Width;
                if (textMeasurement.Width > maxLineWidth)
                {
                    maxLineWidth = textMeasurement.Width;
                }
            }

            SetContentSize(maxLineWidth, _lines.Length * Maths.FontSizeToLineHeight(_fontSize));
        }

        protected override void RenderSelf(IRenderer renderer, float x, float y)
        {
            var lineHeight = Maths.FontSizeToLineHeight(_fontSize);
            switch (_alignment)
            {
                case TextAlignment.Left:
                {
                    // When rendering with left alignment, renderer will take care of line alignment (it's always zero)
                    renderer.RenderText(Font, _text, _fontSize, x, y, ContentWidth, ContentHeight);
                    break;
                }
                case TextAlignment.Center:
                {
                    // When rendering with center alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, x + ContentWidth / 2 - _lineWidths[i] / 2, y + lineHeight * i);
                    }

                    break;
                }
                case TextAlignment.Right:
                {
                    // When rendering with right alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, x + ContentWidth - _lineWidths[i], y + lineHeight * i);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_alignment));
            }
            
            RenderChildren(renderer, x, y);
        }
    }
}