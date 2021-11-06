using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using SCVE.Core.Misc;
using SCVE.Core.Primitives;
using SCVE.Core.Utilities;
using SCVE.UI.Containers;
using SCVE.UI.Elements;
using SCVE.UI.Groups;
using SCVE.UI.Primitive;
using SCVE.UI.StyleValues;

namespace SCVE.UI.UIBuilders
{
    public class XmlUIBuilder : IUIBuilder
    {
        public Component Build(string filePath)
        {
            var xml = File.ReadAllText(filePath);
            var xDocument = XDocument.Parse(xml);
            return Build(xDocument.Root);
        }

        private Component Build(XElement xElement)
        {
            var localName = xElement.Name.LocalName;

            Component component = localName switch
            {
                "color-rect" => ProcessColorRectElement(xElement),
                "text" => ProcessTextElement(xElement),
                "fps-counter" => ProcessFpsCounterElement(xElement),
                "flex" => ProcessFlexElement(xElement),
                "flex-cell" => ProcessFlexCellElement(xElement),
                "align" => ProcessAlignElement(xElement),
                "box" => ProcessBoxElement(xElement),
                "stack" => ProcessStackElement(xElement),
                "clip" => ProcessClipElement(xElement),
                "padding" => ProcessPaddingElement(xElement),
                "glue" => ProcessGlueElement(xElement),
                "template" => ProcessTemplateElement(xElement),
                "button" => ProcessButtonElement(xElement),
                "button-container" => ProcessButtonContainerElement(xElement),
                _ => throw new ScveException($"Unknown component type ({localName})")
            };
            ExtractId(component, xElement);
            component.SetStyle(ExtractStyles(xElement));
            foreach (var element in xElement.Elements())
            {
                component.AddChild(Build(element));
            }

            return component;
        }

        private Component ProcessButtonContainerElement(XElement xElement)
        {
            return new ButtonContainerComponent();
        }

        private Component ProcessButtonElement(XElement xElement)
        {
            var buttonComponent = new ButtonComponent();
            buttonComponent.Text = xElement.Value;
            return buttonComponent;
        }

        private Component ProcessTemplateElement(XElement xElement)
        {
            var nameAttribute = xElement.Attribute("name");
            if (nameAttribute is not null)
            {
                var templateName = nameAttribute.Value;
                var templateComponent = new TemplateComponent
                {
                    Name = templateName
                };
                var builder = new XmlUIBuilder();
                var templateUiFilePath = $"assets/UI/templates/{templateName}.ui.xml";
                if (File.Exists(templateUiFilePath))
                {
                    var build = builder.Build(templateUiFilePath);
                    templateComponent.Component = build;
                    return templateComponent;
                }
                else
                {
                    throw new ScveException($"Template with a given name ({templateName}) was not found in 'templates' directory");
                }
            }
            else
            {
                throw new ScveException("Template Component must declare a name attribute");
            }
        }

        private static Component ProcessGlueElement(XElement xElement)
        {
            var glueComponent = new GlueComponent();

            var directionAttribute = xElement.Attribute("direction");
            if (directionAttribute is not null)
            {
                if (TryParseRawEnum(directionAttribute.Value, out AlignmentDirection direction))
                {
                    glueComponent.Direction = direction;
                }
                else
                {
                    Logger.Warn("Failed to parse direction of GlueComponent");
                }
            }
            else
            {
                // default to horizontal
                glueComponent.Direction = AlignmentDirection.Horizontal;
            }

            return glueComponent;
        }

        private static Component ProcessPaddingElement(XElement xElement)
        {
            var paddingComponent = new PaddingComponent();

            var topAttribute = xElement.Attribute("top");
            if (topAttribute is not null)
            {
                TryParseFloatStyle(paddingComponent.Top, topAttribute.Value);
            }

            var rightAttribute = xElement.Attribute("right");
            if (rightAttribute is not null)
            {
                TryParseFloatStyle(paddingComponent.Right, rightAttribute.Value);
            }

            var bottomAttribute = xElement.Attribute("bottom");
            if (bottomAttribute is not null)
            {
                TryParseFloatStyle(paddingComponent.Bottom, bottomAttribute.Value);
            }

            var leftAttribute = xElement.Attribute("left");
            if (leftAttribute is not null)
            {
                TryParseFloatStyle(paddingComponent.Left, leftAttribute.Value);
            }

            return paddingComponent;
        }

        private static Component ProcessClipElement(XElement xElement)
        {
            return new ClipComponent();
        }

        private static Component ProcessStackElement(XElement xElement)
        {
            return new StackComponent();
        }

        private static Component ProcessBoxElement(XElement xElement)
        {
            return new BoxComponent();
        }

        private static Component ProcessAlignElement(XElement xElement)
        {
            var alignComponent = new AlignComponent();

            var directionAttribute = xElement.Attribute("direction");
            if (directionAttribute is not null)
            {
                TryParseRawEnum(directionAttribute.Value, out AlignmentDirection direction);
                alignComponent.Direction = direction;
            }
            else
            {
                alignComponent.Direction = AlignmentDirection.Horizontal;
            }

            var behaviorAttribute = xElement.Attribute("behavior");
            if (behaviorAttribute is not null)
            {
                TryParseRawEnum(behaviorAttribute.Value, out AlignmentBehavior behavior);
                alignComponent.Behavior = behavior;
            }
            else
            {
                alignComponent.Behavior = AlignmentBehavior.Start;
            }

            return alignComponent;
        }

        private static Component ProcessFlexCellElement(XElement xElement)
        {
            var flexCell = new FlexCell();
            var flexAttribute = xElement.Attribute("flex");
            if (flexAttribute is not null)
            {
                TryParseRawFloat(flexAttribute.Value, out var flex);
                flexCell.Flex = flex;
            }
            else
            {
                flexCell.Flex = 1;
            }

            return flexCell;
        }

        private static Component ProcessFlexElement(XElement xElement)
        {
            var flex = new FlexComponent();
            var directionAttribute = xElement.Attribute("direction");
            if (directionAttribute is not null)
            {
                TryParseRawEnum(directionAttribute.Value, out AlignmentDirection direction);
                flex.Direction = direction;
            }
            else
            {
                flex.Direction = AlignmentDirection.Horizontal;
            }

            return flex;
        }

        private static Component ProcessFpsCounterElement(XElement xElement)
        {
            return new FpsComponent();
        }

        private static Component ProcessTextElement(XElement xElement)
        {
            var textComponent = new TextComponent();
            textComponent._text = xElement.Value;
            return textComponent;
        }

        private void ExtractId(Component destination, XElement source)
        {
            var idAttribute = source.Attribute("id");
            if (idAttribute is not null)
            {
                var elementId = idAttribute.Value;
                destination.Id = elementId;
                Logger.Warn($"Found Id for {destination.GetType().Name} element: {elementId}.");
            }
            else
            {
                destination.Id = "";
                Logger.Warn($"No Id specified for {destination.GetType().Name} element.");
            }
        }

        private static ColorRectComponent ProcessColorRectElement(XElement xElement)
        {
            return new ColorRectComponent();
        }

        private static ComponentStyle ExtractStyles(XElement xElement)
        {
            var defaultStyle = ComponentStyle.Default;
            FloatStyleValue width = new FloatStyleValue(defaultStyle.Width.Value);
            FloatStyleValue height = new FloatStyleValue(defaultStyle.Height.Value);
            FloatStyleValue maxWidth = new FloatStyleValue(defaultStyle.MaxWidth.Value);
            FloatStyleValue maxHeight = new FloatStyleValue(defaultStyle.MaxHeight.Value);
            FloatStyleValue minWidth = new FloatStyleValue(defaultStyle.MinWidth.Value);
            FloatStyleValue minHeight = new FloatStyleValue(defaultStyle.MinHeight.Value);
            StyleValue<AlignmentDirection> alignmentDirection = new StyleValue<AlignmentDirection>(defaultStyle.AlignmentDirection.Value);
            StyleValue<AlignmentBehavior> horizontalAlignmentBehavior = new StyleValue<AlignmentBehavior>(defaultStyle.HorizontalAlignmentBehavior.Value);
            StyleValue<AlignmentBehavior> verticalAlignmentBehavior = new StyleValue<AlignmentBehavior>(defaultStyle.VerticalAlignmentBehavior.Value);
            ColorStyleValue primaryColor = new ColorStyleValue(new ColorRgba(defaultStyle.PrimaryColor.Value));
            ColorStyleValue textColor = new ColorStyleValue(new ColorRgba(defaultStyle.TextColor.Value));
            StringStyleValue fontFileName = new StringStyleValue(defaultStyle.FontFileName.Value);
            FloatStyleValue fontSize = new FloatStyleValue(defaultStyle.FontSize.Value);
            StyleValue<TextAlignment> textAlignment = new StyleValue<TextAlignment>(defaultStyle.TextAlignment.Value);

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
                        if (!TryParseColor(primaryColor, styleValue))
                        {
                            Logger.Warn($"Failed to parse primary-color style value ({styleValue})");
                        }

                        break;
                    }
                    case "text-color":
                    {
                        if (!TryParseColor(textColor, styleValue))
                        {
                            Logger.Warn($"Failed to parse text-color style value ({styleValue})");
                        }

                        break;
                    }
                    case "font-file-name":
                    {
                        fontFileName.Value = styleValue;
                        break;
                    }
                    case "font-size":
                    {
                        if (TryParseRawFloat(styleValue, out var fontSizeParsed))
                        {
                            fontSize.Value = fontSizeParsed;
                        }
                        else
                        {
                            Logger.Warn("Failed to parse font-size style value");
                        }

                        break;
                    }
                    case "text-alignment":
                    {
                        if (!TryParseEnumStyle(textAlignment, styleValue))
                        {
                            Logger.Warn("Failed to parse text-alignment style value");
                        }

                        break;
                    }
                }
            }

            return new(
                width: width,
                height: height,
                maxWidth: maxWidth,
                maxHeight: maxHeight,
                minWidth: minWidth,
                minHeight: minHeight,
                alignmentDirection: alignmentDirection,
                horizontalAlignmentBehavior: horizontalAlignmentBehavior,
                verticalAlignmentBehavior: verticalAlignmentBehavior,
                primaryColor: primaryColor,
                textColor: textColor,
                fontFileName: fontFileName,
                fontSize: fontSize,
                textAlignment: textAlignment
            );
        }

        private static bool TryParseColor(ColorStyleValue colorStyleValue, string styleValue)
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
                    Logger.Warn($"Failed to parse rgba255 values ({styleValue})");
                    return false;
                }

                colorStyleValue.Value.R = r / 255;
                colorStyleValue.Value.G = g / 255;
                colorStyleValue.Value.B = b / 255;
                colorStyleValue.Value.A = a / 255;
                return true;
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
                    Logger.Warn($"Failed to parse rgba01 values ({styleValue})");
                    return false;
                }

                colorStyleValue.Value.R = r;
                colorStyleValue.Value.G = g;
                colorStyleValue.Value.B = b;
                colorStyleValue.Value.A = a;
                return true;
            }
            else if (styleValue.StartsWith('#'))
            {
                var substring = styleValue.Substring("#".Length);
                if (substring.Length == 6)
                {
                    // #AABBCCDD
                    string rString = substring[0..2];
                    string gString = substring[2..4];
                    string bString = substring[4..6];

                    colorStyleValue.Value.R = Convert.ToInt32(rString, 16) / 255f;
                    colorStyleValue.Value.G = Convert.ToInt32(gString, 16) / 255f;
                    colorStyleValue.Value.B = Convert.ToInt32(bString, 16) / 255f;
                    colorStyleValue.Value.A = 1;
                    return true;
                }
                else if (substring.Length == 8)
                {
                    // #AABBCCDD
                    string rString = substring[0..2];
                    string gString = substring[2..4];
                    string bString = substring[4..6];
                    string aString = substring[6..8];

                    colorStyleValue.Value.R = Convert.ToInt32(rString, 16) / 255f;
                    colorStyleValue.Value.G = Convert.ToInt32(gString, 16) / 255f;
                    colorStyleValue.Value.B = Convert.ToInt32(bString, 16) / 255f;
                    colorStyleValue.Value.A = Convert.ToInt32(aString, 16) / 255f;
                    return true;
                }
                else
                {
                    Logger.Warn($"Failed to parse # values ({styleValue})");
                    return false;
                }
            }
            else
            {
                return false;
            }
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

        private static bool TryParseRawEnum<T>(string value, out T result) where T : struct, Enum
        {
            if (Enum.TryParse(value, true, out T parsed))
            {
                result = parsed;
                return true;
            }
            else
            {
                Logger.Warn($"Failed to parse enum property! Value ({value})");
                result = default;
                return false;
            }
        }
    }
}