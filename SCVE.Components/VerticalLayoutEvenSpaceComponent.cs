using SCVE.Core;
using SCVE.Core.Primitives;

namespace SCVE.Components
{
    public class VerticalLayoutEvenSpaceComponent : LayoutComponent
    {
        private void ConstraintChildren()
        {
            var componentHeight = PixelHeight / Children.Count;

            var componentWidth = PixelWidth;
            
            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.X = X;
                child.Y = Y + componentHeight * index;
                child.PixelWidth = componentWidth;
                child.PixelHeight = componentHeight;
                child.OnResize();
            }
        }

        public override void OnResize()
        {
            base.OnResize();
            ConstraintChildren();
        }

        public override void AddChild(Component component)
        {
            base.AddChild(component);
            ConstraintChildren();
        }
    }
}