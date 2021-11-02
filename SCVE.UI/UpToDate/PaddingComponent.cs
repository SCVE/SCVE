using SCVE.UI.StyleValues;
using SCVE.UI.Visitors;

namespace SCVE.UI.UpToDate
{
    public class PaddingComponent : ContainerComponent
    {
        public FloatStyleValue Top { get; set; }
        public FloatStyleValue Right { get; set; }
        public FloatStyleValue Bottom { get; set; }
        public FloatStyleValue Left { get; set; }

        public PaddingComponent()
        {
            Top    = new();
            Right  = new();
            Bottom = new();
            Left   = new();
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            float left = Left.Flatten(availableWidth);
            float right = Right.Flatten(availableWidth);
            float top = Top.Flatten(availableHeight);
            float bottom = Bottom.Flatten(availableHeight);
            
            Component.Measure(availableWidth - left - right, availableHeight - top - bottom);

            DesiredWidth  = availableWidth;
            DesiredHeight = availableHeight;
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            base.Arrange(x, y, availableWidth, availableHeight);
            
            float left = Left.Flatten(availableWidth);
            float right = Right.Flatten(availableWidth);
            float top = Top.Flatten(availableHeight);
            float bottom = Bottom.Flatten(availableHeight);
            
            Component.Arrange(x + left, y + top, availableWidth - left - right, availableHeight - top - bottom);
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}