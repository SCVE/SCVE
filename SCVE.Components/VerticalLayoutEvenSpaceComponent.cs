using SCVE.Core;
using SCVE.Core.Primitives;

namespace SCVE.Components
{
    public class VerticalLayoutEvenSpaceComponent : LayoutComponent
    {
        public override void OnResize()
        {
            var componentHeight = PixelHeight / Children.Count;

            var componentWidth = PixelWidth;
            
            var scale = ScveMatrix4X4.CreateScale(componentWidth, componentHeight);
            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.X = X;
                child.Y = Y + componentHeight * index;
                child.PixelWidth = componentWidth;
                child.PixelHeight = componentHeight;
                child.ModelMatrix.MakeIdentity().Multiply(scale).Multiply(ScveMatrix4X4.CreateTranslation(child.X, child.Y));
                child.OnResize();
            }
        }
        
        public override void AddChild(Component component)
        {
            base.AddChild(component);
            OnResize();
        }
    }
}