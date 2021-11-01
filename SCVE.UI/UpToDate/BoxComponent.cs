using System;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.UpToDate
{
    /// <summary>
    /// Box Component fills all given empty space and shrinks single child to a box
    /// </summary>
    public class BoxComponent : ContainerComponent
    {
        public override void Measure(float availableWidth, float availableHeight)
        {
            DesiredWidth  = Style.Width.Flatten(availableWidth);
            DesiredHeight = Style.Height.Flatten(availableHeight);

            if (!Component.HasConstMeasure)
            {
                Component.Measure(DesiredWidth, DesiredHeight);
            }

            if (Component.DesiredWidth > DesiredWidth || Component.DesiredHeight > DesiredHeight)
            {
                float downscaleFactor = MathF.Min(Component.DesiredWidth / DesiredWidth, Component.DesiredHeight / DesiredHeight);
                float destWidth = Component.DesiredWidth * downscaleFactor;
                float destHeight = Component.DesiredHeight * downscaleFactor;
                Component.Measure(destWidth, destHeight);
            }
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}