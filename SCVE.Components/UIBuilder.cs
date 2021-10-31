using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using SCVE.Components.UpToDate;
using SCVE.Core.Misc;
using SCVE.Core.Primitives;
using SCVE.Core.UI;
using SCVE.Core.UI.StyleValues;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class UIBuilder
    {
        public static Component Build(string xml)
        {
            var xDocument = XDocument.Parse(xml);
            return Build(xDocument.Root);
        }

        public static Component Build(XElement xElement)
        {
            var localName = xElement.Name.LocalName;

            Component component = localName switch
            {
                "color-rect" => ProcessColorRectElement(xElement),
                "text" => ProcessTextElement(xElement),
                "outline" => ProcessOutlineElement(xElement),
                "fps-counter" => ProcessFpsCounterElement(xElement),
                "group" => ProcessGroupElement(xElement),
                _ => throw new ScveException($"Unknown component type ({localName})")
            };
            component.SetStyle(ExtractStyles(xElement));
            foreach (var element in xElement.Elements())
            {
                component.AddChild(Build(element));
            }

            return component;
        }

        private static Component ProcessGroupElement(XElement xElement)
        {
            return new GroupComponent();
        }

        private static Component ProcessFpsCounterElement(XElement xElement)
        {
            return new FpsCounter();
        }

        private static Component ProcessOutlineElement(XElement xElement)
        {
            return new OutlineComponent();
        }

        private static Component ProcessTextElement(XElement xElement)
        {
            string fontFileName = xElement.Attribute("font-file-name")?.Value ?? "arial.ttf";
            float fontSize = float.Parse(xElement.Attribute("font-size")?.Value ?? "14", NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            string text = xElement.Attribute("text")?.Value ?? "unknown";

            TextAlignment alignment = Enum.Parse<TextAlignment>(xElement.Attribute("alignment")?.Value ?? "left", true);

            return new TextComponent(
                fontFileName,
                fontSize,
                text,
                alignment
            );
        }

        private static ColorRectComponent ProcessColorRectElement(XElement xElement)
        {
            return new ColorRectComponent();
        }

        private static ComponentStyle ExtractStyles(XElement xElement)
        {
            var defaultStyle = ComponentStyle.Default;
            FloatStyleValue width = defaultStyle.Width;
            FloatStyleValue height = defaultStyle.Height;
            FloatStyleValue maxWidth = defaultStyle.MaxWidth;
            FloatStyleValue maxHeight = defaultStyle.MaxHeight;
            FloatStyleValue minWidth = defaultStyle.MinWidth;
            FloatStyleValue minHeight = defaultStyle.MinHeight;
            StyleValue<AlignmentDirection> alignmentDirection = defaultStyle.AlignmentDirection;
            StyleValue<AlignmentBehavior> horizontalAlignmentBehavior = defaultStyle.HorizontalAlignmentBehavior;
            StyleValue<AlignmentBehavior> verticalAlignmentBehavior = defaultStyle.VerticalAlignmentBehavior;
            ColorStyleValue primaryColor = new ColorStyleValue(new ColorRgba(defaultStyle.PrimaryColor.Value));

            if (xElement.Attribute("style") is not { } attribute)
            {
                return defaultStyle;
            }

            var tokens = attribute.Value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (var i = 0; i < tokens.Length; i++)
            {
                var tokenPair = tokens[i].Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (tokenPair.Length != 2)
                {
                    Logger.Warn($"Invalid token pair ({tokens[i]})");
                    continue;
                }

                var styleName = tokenPair[0].Trim();
                var styleValue = tokenPair[1];

                switch (styleName)
                {
                    case "width":
                    {
                        if (!TryParseFloatStyle(width, styleValue))
                        {
                            Logger.Warn("Failed to parse width style");
                        }

                        break;
                    }
                    case "height":
                    {
                        if (!TryParseFloatStyle(height, styleValue))
                        {
                            Logger.Warn("Failed to parse height style");
                        }

                        break;
                    }
                    case "max-width":
                    {
                        if (!TryParseFloatStyle(maxWidth, styleValue))
                        {
                            Logger.Warn("Failed to parse max-width style");
                        }

                        break;
                    }
                    case "max-height":
                    {
                        if (!TryParseFloatStyle(maxHeight, styleValue))
                        {
                            Logger.Warn("Failed to parse max-height style");
                        }

                        break;
                    }
                    case "min-width":
                    {
                        if (!TryParseFloatStyle(minWidth, styleValue))
                        {
                            Logger.Warn("Failed to parse min-width style");
                        }

                        break;
                    }
                    case "min-height":
                    {
                        if (!TryParseFloatStyle(minHeight, styleValue))
                        {
                            Logger.Warn("Failed to parse min-height style");
                        }

                        break;
                    }
                    case "alignment-direction":
                    {
                        if (!TryParseEnumStyle(alignmentDirection, styleValue))
                        {
                            Logger.Warn($"Failed to parse (horizontal-alignment) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "horizontal-alignment-behavior":
                    {
                        if (!TryParseEnumStyle(horizontalAlignmentBehavior, styleValue))
                        {
                            Logger.Warn($"Failed to parse (horizontal-alignment-behavior) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "vertical-alignment-behavior":
                    {
                        if (!TryParseEnumStyle(verticalAlignmentBehavior, styleValue))
                        {
                            Logger.Warn($"Failed to parse (vertical-alignment-behavior) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "primary-color":
                    {
                        if (styleValue.StartsWith("rgba255"))
                        {
                            var substring = styleValue.Substring("rgba255".Length + 1);
                            substring = substring.Substring(0, substring.LastIndexOf(')'));
                            var values = substring.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                            float r, g, b, a;
                            if (!TryParseRawFloat(values[0], out r) ||
                                !TryParseRawFloat(values[1], out g) ||
                                !TryParseRawFloat(values[2], out b) ||
                                !TryParseRawFloat(values[3], out a)
                            )
                            {
                                throw new ScveException($"Failed to parse rgba255 values ({styleValue})");
                            }

                            primaryColor.Value.R = r / 255;
                            primaryColor.Value.G = g / 255;
                            primaryColor.Value.B = b / 255;
                            primaryColor.Value.A = a / 255;
                        }
                        else if (styleValue.StartsWith("rgba01"))
                        {
                            var substring = styleValue.Substring("rgba01".Length + 1);
                            substring = substring.Substring(0, substring.LastIndexOf(')'));
                            var values = substring.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                            float r, g, b, a;
                            if (!TryParseRawFloat(values[0], out r) ||
                                !TryParseRawFloat(values[1], out g) ||
                                !TryParseRawFloat(values[2], out b) ||
                                !TryParseRawFloat(values[3], out a)
                            )
                            {
                                throw new ScveException($"Failed to parse rgba01 values ({styleValue})");
                            }

                            primaryColor.Value.R = r;
                            primaryColor.Value.G = g;
                            primaryColor.Value.B = b;
                            primaryColor.Value.A = a;
                        }

                        break;
                    }
                }
            }

            return new(
                width,
                height,
                maxWidth: maxWidth,
                maxHeight: maxHeight,
                minWidth: minWidth,
                minHeight: minHeight,
                alignmentDirection: alignmentDirection,
                horizontalAlignmentBehavior: horizontalAlignmentBehavior,
                verticalAlignmentBehavior: verticalAlignmentBehavior,
                primaryColor: primaryColor
            );
        }

        private static bool TryParseRawFloat(string styleValue, out float value)
        {
            return float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
        }

        private static bool TryParseFloatStyle(FloatStyleValue style, string styleValue)
        {
            if (styleValue.EndsWith('%'))
            {
                if (TryParseRawFloat(styleValue.Substring(0, styleValue.Length - 1), out var value))
                {
                    style.IsRelative = true;
                    style.IsAbsolute = false;
                    style.Value      = value;
                    style.Specified  = true;
                    return true;
                }
                else
                {
                    Logger.Warn($"Failed to parse float value with % ({styleValue})");
                    style.Specified = false;
                    return false;
                }
            }
            else
            {
                if (TryParseRawFloat(styleValue, out var value))
                {
                    style.IsRelative = false;
                    style.IsAbsolute = true;
                    style.Value      = value;
                    style.Specified  = true;
                    return true;
                }
                else
                {
                    Logger.Warn($"Failed to parse absolute float value ({styleValue})");
                    style.Specified = false;
                    return false;
                }
            }
        }

        private static bool TryParseEnumStyle<T>(StyleValue<T> style, string styleValue) where T : struct, Enum
        {
            if (Enum.TryParse(styleValue, true, out T parsed))
            {
                style.Value     = parsed;
                style.Specified = true;
                return true;
            }
            else
            {
                Logger.Warn($"Failed to parse enum style property! Value ({styleValue})");
                style.Specified = false;
                return false;
            }
        }
    }
}