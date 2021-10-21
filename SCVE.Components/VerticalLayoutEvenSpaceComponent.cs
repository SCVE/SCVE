using SCVE.Core;

namespace SCVE.Components
{
    public class VerticalLayoutEvenSpaceComponent : LayoutComponent
    {
        protected override void ConstraintChildren()
        {
            var componentHeight = PixelHeight / Children.Count;

            var componentWidth = PixelWidth;

            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.SetPositionAndSize(
                    x: X,
                    y: Y + componentHeight * index,
                    width: componentWidth,
                    height: componentHeight
                );
            }
        }
    }
}