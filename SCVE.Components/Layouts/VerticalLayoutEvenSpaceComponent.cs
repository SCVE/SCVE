namespace SCVE.Components.Layouts
{
    public class VerticalLayoutEvenSpaceComponent : LayoutComponent
    {
        protected override void ConstraintChildren()
        {
            var cellHeight = PixelHeight / Children.Count;

            var cellWidth = PixelWidth;
            
            ValidateDividersCount();

            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.SetPositionAndSize(
                    x: X,
                    y: Y + cellHeight * index,
                    width: cellWidth,
                    height: cellHeight
                );
                
                if (index != Children.Count - 1)
                {
                    Dividers[index].SetPositionAndSize(
                        x: X,
                        y: Y + cellHeight * (index + 1) - Divider.DefaultHeight / 2,
                        width: cellWidth,
                        height: Divider.DefaultHeight
                    );
                }
            }
        }
    }
}