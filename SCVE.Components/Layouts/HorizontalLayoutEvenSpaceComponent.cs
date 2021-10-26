namespace SCVE.Components.Layouts
{
    public class HorizontalLayoutEvenSpaceComponent : LayoutComponent
    {
        protected override void ConstraintChildren()
        {
            var cellHeight = PixelHeight;

            var cellWidth = PixelWidth / Children.Count;
            
            ValidateDividersCount();

            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];

                child.SetPositionAndSize(
                    x: X + cellWidth * index,
                    y: Y,
                    width: cellWidth,
                    height: cellHeight
                );
                
                if (index != Children.Count - 1)
                {
                    Dividers[index].SetPositionAndSize(
                        x: X + cellWidth * (index + 1) - Divider.DefaultWidth / 2,
                        y: Y,
                        width: Divider.DefaultWidth,
                        height: cellHeight
                    );
                }
            }
        }
    }
}