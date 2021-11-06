using System;
using SCVE.Core.Main;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.Primitive
{
    public class TextComponent : Component
    {
        public ScveFont Font { get; set; }

        private string _fontFileName;
        private float _fontSize;
        internal string _text = "";

        private string[] _lines;
        private float[] _lineWidths;

        private TextAlignment _alignment;

        private bool _measureValid;

        public TextComponent()
        {
            Logger.Construct(nameof(TextComponent));
        }

        public override void OnSetStyle()
        {
            base.OnSetStyle();

            _measureValid = false;
            _fontFileName = Style.FontFileName.Value;
            _fontSize     = Style.FontSize.Value;
            _alignment    = Style.TextAlignment.Value;
        }

        public override Component PickComponentByPosition(float x, float y)
        {
            // By default, raw text can not receive any inputs and therefore be picked, because it's just a text.
            // TODO: Wrap into a label component
            return null;
        }

        public override void Init()
        {
            base.Init();
            
            Font = Engine.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));
        }

        public string GetText()
        {
            return _text;
        }

        public void SetText(string text)
        {
            _text         = text;
            _measureValid = false;
            SubtreeUpdated();
        }

        private void Rebuild()
        {
            _lines      = _text.Split('\n');
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

            DesiredWidth  = maxLineWidth;
            DesiredHeight = _lines.Length * Maths.FontSizeToLineHeight(_fontSize);
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            if (!_measureValid)
            {
                Rebuild();
                _measureValid = true;
            }
            else
            {
                // All measures are done in Rebuild()
            }
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X      = x;
            Y      = y;
            Width  = DesiredWidth;
            Height = DesiredHeight;
        }

        public override void RenderSelf(IRenderer renderer)
        {
            var lineHeight = Maths.FontSizeToLineHeight(_fontSize);
            switch (_alignment)
            {
                case TextAlignment.Left:
                {
                    // When rendering with left alignment, renderer will take care of line alignment (it's always zero)
                    renderer.RenderText(Font, _text, _fontSize, X, Y, Style.TextColor.Value);
                    break;
                }
                case TextAlignment.Center:
                {
                    // When rendering with center alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, X + Width / 2 - _lineWidths[i] / 2, Y + lineHeight * i, Style.TextColor.Value);
                    }

                    break;
                }
                case TextAlignment.Right:
                {
                    // When rendering with right alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, X + Width - _lineWidths[i], Y + lineHeight * i, Style.TextColor.Value);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_alignment));
            }
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}