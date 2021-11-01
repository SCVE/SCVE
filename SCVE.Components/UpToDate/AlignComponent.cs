using System;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class AlignComponent : ContainerComponent
    {
        public AlignmentBehavior Behavior { get; set; }
        public AlignmentDirection Direction { get; set; }

        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent($"{nameof(AlignComponent)} {X}:{Y}:{Width}:{Height}:{Direction}-{Behavior}", indent);
            Component.PrintComponentTree(indent + 1);
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            base.Arrange(x, y, availableWidth, availableHeight);

            if (Direction == AlignmentDirection.Horizontal)
            {
                // Direction Horizontal
                switch (Behavior)
                {
                    case AlignmentBehavior.Start:
                        Component.Arrange(X, Y, Component.DesiredWidth, Component.DesiredHeight);
                        break;
                    case AlignmentBehavior.Center:
                        Component.Arrange(X + Width / 2 - Component.DesiredWidth / 2, Y, Component.DesiredWidth, Component.DesiredHeight);
                        break;
                    case AlignmentBehavior.End:
                        Component.Arrange(X + Width - Component.DesiredWidth, Y, Component.DesiredWidth, Component.DesiredHeight);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (Direction == AlignmentDirection.Vertical)
            {
                // Direction Vertical
                switch (Behavior)
                {
                    case AlignmentBehavior.Start:
                        Component.Arrange(X, Y, Component.DesiredWidth, Component.DesiredHeight);
                        break;
                    case AlignmentBehavior.Center:
                        Component.Arrange(X, Y + Height / 2 - Component.DesiredHeight / 2, Component.DesiredWidth, Component.DesiredHeight);
                        break;
                    case AlignmentBehavior.End:
                        Component.Arrange(X, Y + Height - Component.DesiredHeight, Component.DesiredWidth, Component.DesiredHeight);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}