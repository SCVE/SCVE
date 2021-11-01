using System;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    /// <summary>
    /// Box Component fills all given empty space and shrinks single child to a box
    /// </summary>
    public class BoxComponent : ContainerComponent
    {
        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent($"{nameof(BoxComponent)} {X}:{Y}:{Width}:{Height}", indent);
            Component.PrintComponentTree(indent + 1);
        }

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
    }
}