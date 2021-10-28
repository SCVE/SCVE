using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SCVE.Components.Layouts;
using SCVE.Core;
using SCVE.Core.Primitives;

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
                "vertical-layout" => ProcessVerticalLayoutElement(xElement),
                "horizontal-layout" => ProcessHorizontalLayoutElement(),
                "color-rect" => ProcessColorRectElement(xElement),
                "text" => ProcessTextElement(xElement),
                "outline" => ProcessOutlineElement(xElement),
                _ => throw new ScveException($"Unknown component type ({localName})")
            };
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
            float fontSize = Convert.ToSingle(xElement.Attribute("font-size")?.Value ?? "14");
            string text = xElement.Attribute("text")?.Value ?? "unknown";

            return new TextViaAtlasComponent(
                fontFileName, fontSize, text
            );
        }

        private static FullFlexColoredRectComponent ProcessColorRectElement(XElement xElement)
        {
            float r = Convert.ToSingle(xElement.Attribute("r")?.Value ?? "0");
            float g = Convert.ToSingle(xElement.Attribute("g")?.Value ?? "0");
            float b = Convert.ToSingle(xElement.Attribute("b")?.Value ?? "0");
            float a = Convert.ToSingle(xElement.Attribute("a")?.Value ?? "0");

            return new FullFlexColoredRectComponent(
                new ColorRgba(r, g, b, a)
            );
        }

        private static HorizontalLayoutEvenSpaceComponent ProcessHorizontalLayoutElement()
        {
            return new HorizontalLayoutEvenSpaceComponent();
        }

        private static VerticalLayoutEvenSpaceComponent ProcessVerticalLayoutElement(XElement xElement)
        {
            return new VerticalLayoutEvenSpaceComponent();
        }

        private static EmptyComponent ProcessEmptyElement(XElement xElement)
        {
            return new EmptyComponent();
        }
    }
}