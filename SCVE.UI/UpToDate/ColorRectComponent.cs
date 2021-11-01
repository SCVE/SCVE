﻿using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.UpToDate
{
    /// <summary>
    /// Color Rect Component is a basically a 2D rectangle, it occupies only the size specified in the style
    /// </summary>
    public class ColorRectComponent : Component
    {
        public ColorRectComponent()
        {
            Logger.Construct(nameof(ColorRectComponent));
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            DesiredWidth  = Style.Width.Flatten(availableWidth);
            DesiredHeight = Style.Height.Flatten(availableHeight);
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X = x;
            Y = y;
            
            Width  = Style.Width.Flatten(availableWidth);
            Height = Style.Height.Flatten(availableHeight);
        }

        public override void RenderSelf(IRenderer renderer)
        {
            renderer.RenderColorRect(X, Y, Width, Height, Style.PrimaryColor.Value);
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}