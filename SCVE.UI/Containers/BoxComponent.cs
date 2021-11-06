using SCVE.UI.Visitors;

namespace SCVE.UI.Containers
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

            Component.Measure(DesiredWidth, DesiredHeight);
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X      = x;
            Y      = y;
            Width  = Style.Width.Flatten(availableWidth);
            Height = Style.Height.Flatten(availableHeight);
            Component.Arrange(x, y, Width, Height);
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}