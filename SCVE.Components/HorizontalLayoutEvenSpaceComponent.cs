namespace SCVE.Components
{
    public class HorizontalLayoutEvenSpaceComponent : LayoutComponent
    {
        protected override void ConstraintChildren()
        {
            var componentHeight = PixelHeight;

            var componentWidth = PixelWidth / Children.Count;

            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];

                child.SetPositionAndSize(
                    x: X + componentWidth * index,
                    y: Y,
                    width: componentWidth,
                    height: componentHeight
                );
            }
        }
    }
}