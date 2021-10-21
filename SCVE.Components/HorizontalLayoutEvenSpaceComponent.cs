using SCVE.Core;
using SCVE.Core.Primitives;

namespace SCVE.Components
{
    public class HorizontalLayoutEvenSpaceComponent : LayoutComponent
    {
        private void ConstraintChildren()
        {
            var componentHeight = PixelHeight;

            var componentWidth = PixelWidth / Children.Count;
            
            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.X = X + componentWidth * index;
                child.Y = Y;
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