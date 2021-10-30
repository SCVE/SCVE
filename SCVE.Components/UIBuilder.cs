using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using SCVE.Components.UpToDate;
using SCVE.Core.Misc;
using SCVE.Core.Primitives;
using SCVE.Core.UI;
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
                "empty" => ProcessEmptyElement(xElement),
                "color-rect" => ProcessColorRectElement(xElement),
                "text" => ProcessTextElement(xElement),
                "outline" => ProcessOutlineElement(xElement),
                _ => throw new ScveException($"Unknown component type ({localName})")
            };
            component.SetStyle(ExtractStyles(xElement));
            foreach (var element in xElement.Elements())
            {
                component.AddChild(Build(element));
            }

            return component;
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

        private static EmptyComponent ProcessEmptyElement(XElement xElement)
        {
            return new EmptyComponent();
        }

        private static ComponentStyle ExtractStyles(XElement xElement)
        {
            float width = ComponentStyle.Default.Width;
            float height = ComponentStyle.Default.Height;
            float maxWidth = ComponentStyle.Default.MaxWidth;
            float maxHeight = ComponentStyle.Default.MaxHeight;
            float minWidth = ComponentStyle.Default.MinWidth;
            float minHeight = ComponentStyle.Default.MinHeight;
            AlignmentDirection alignmentDirection = ComponentStyle.Default.AlignmentDirection;
            AlignmentBehavior horizontalAlignmentBehavior = ComponentStyle.Default.HorizontalAlignmentBehavior;
            AlignmentBehavior verticalAlignmentBehavior = ComponentStyle.Default.VerticalAlignmentBehavior;
            ColorRgba primaryColor = new ColorRgba(ComponentStyle.Default.PrimaryColor);

            if (xElement.Attribute("style") is not { } attribute)
            {
                return ComponentStyle.Default;
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
                        if (!float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out width))
                        {
                            Logger.Warn($"Failed to parse (width) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "height":
                    {
                        if (!float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out height))
                        {
                            Logger.Warn($"Failed to parse (height) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "max-width":
                    {
                        if (!float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out maxWidth))
                        {
                            Logger.Warn($"Failed to parse (max-width) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "max-height":
                    {
                        if (!float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out maxHeight))
                        {
                            Logger.Warn($"Failed to parse (max-height) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "min-width":
                    {
                        if (!float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out minWidth))
                        {
                            Logger.Warn($"Failed to parse (min-width) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "min-height":
                    {
                        if (!float.TryParse(styleValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out minHeight))
                        {
                            Logger.Warn($"Failed to parse (min-height) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "alignment-direction":
                    {
                        if (!Enum.TryParse(styleValue, true, out alignmentDirection))
                        {
                            Logger.Warn($"Failed to parse (horizontal-alignment) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "horizontal-alignment-behavior":
                    {
                        if (!Enum.TryParse(styleValue, true, out horizontalAlignmentBehavior))
                        {
                            Logger.Warn($"Failed to parse (horizontal-alignment) style property! Value ({styleValue})");
                        }

                        break;
                    }
                    case "vertical-alignment-behavior":
                    {
                        if (!Enum.TryParse(styleValue, true, out verticalAlignmentBehavior))
                        {
                            Logger.Warn($"Failed to parse (vertical-alignment) style property! Value ({styleValue})");
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
                            if (!float.TryParse(values[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out r)||
                                !float.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out g)||
                                !float.TryParse(values[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out b)||
                                !float.TryParse(values[3], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out a)
                                )
                            {
                                throw new ScveException($"Failed to parse rgba values ({styleValue})");
                            }

                            primaryColor.R = r / 255;
                            primaryColor.G = g / 255;
                            primaryColor.B = b / 255;
                            primaryColor.A = a / 255;
                        }
                        else if (styleValue.StartsWith("rgba01"))
                        {
                            var substring = styleValue.Substring("rgba01".Length + 1);
                            substring = substring.Substring(0, substring.LastIndexOf(')'));
                            var values = substring.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                            float r, g, b, a;
                            if (!float.TryParse(values[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out r)||
                                !float.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out g)||
                                !float.TryParse(values[2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out b)||
                                !float.TryParse(values[3], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out a)
                            )
                            {
                                throw new ScveException($"Failed to parse rgba values ({styleValue})");
                            }

                            primaryColor.R = r;
                            primaryColor.G = g;
                            primaryColor.B = b;
                            primaryColor.A = a;
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
    }
}