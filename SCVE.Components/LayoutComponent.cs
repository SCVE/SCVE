using SCVE.Core;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public abstract class LayoutComponent : Component
    {
        protected abstract void ConstraintChildren();
        
        protected override void OnResized()
        {
            base.OnResized();
            ConstraintChildren();
        }

        public override void AddChild(Component component)
        {
            base.AddChild(component);
            ConstraintChildren();
        }

        public override void Render(IRenderer renderer)
        {
            foreach (var child in Children)
            {
                child.Render(renderer);
            }
        }
    }
}