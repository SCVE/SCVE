﻿using System;
using SCVE.Core.App;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class TextComponent : Component
    {
        public ScveFont Font { get; set; }

        private string _fontFileName;
        private float _fontSize;
        private string _text;

        private string[] _lines;
        private float[] _lineWidths;

        private TextAlignment _alignment;

        public override bool HasConstMeasure { get; set; } = true;

        public TextComponent(string fontFileName, float fontSize, string text, TextAlignment alignment)
        {
            Logger.Construct(nameof(TextComponent));
            _fontFileName = fontFileName;
            _fontSize     = fontSize;
            _text         = text;
            _alignment    = alignment;

            Application.Instance.Input.Scroll += InputOnScroll;

            Font = Application.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));

            Rebuild();
        }

        private void InputOnScroll(float arg1, float arg2)
        {
            Logger.Warn($"Scrolled {arg2}");
            _fontSize += arg2;
            Font      =  Application.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));
            Rebuild();
        }

        public void SetText(string text)
        {
            _text = text;
            Rebuild();
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

            SubtreeUpdated();
        }

        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent(nameof(TextComponent), indent);
        }

        public override void RenderSelf(IRenderer renderer)
        {
            var lineHeight = Maths.FontSizeToLineHeight(_fontSize);
            switch (_alignment)
            {
                case TextAlignment.Left:
                {
                    // When rendering with left alignment, renderer will take care of line alignment (it's always zero)
                    renderer.RenderText(Font, _text, _fontSize, X, Y, Style.PrimaryColor.Value);
                    break;
                }
                case TextAlignment.Center:
                {
                    // When rendering with center alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, X + Width / 2 - _lineWidths[i] / 2, Y + lineHeight * i, Style.PrimaryColor.Value);
                    }

                    break;
                }
                case TextAlignment.Right:
                {
                    // When rendering with right alignment, we need to render line by line, telling renderer where to start
                    for (var i = 0; i < _lines.Length; i++)
                    {
                        renderer.RenderText(Font, _lines[i], _fontSize, X + Width - _lineWidths[i], Y + lineHeight * i, Style.PrimaryColor.Value);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(_alignment));
            }
        }
    }
}