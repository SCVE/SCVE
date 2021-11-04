using System;
using SCVE.UI.Visitors;

namespace SCVE.UI.Containers
{
    /// <summary>
    /// Align Component fills all given empty space and aligns single child inside it
    /// </summary>
    public class AlignComponent : ContainerComponent
    {
        public AlignmentBehavior Behavior { get; set; }
        public AlignmentDirection Direction { get; set; }

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

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}