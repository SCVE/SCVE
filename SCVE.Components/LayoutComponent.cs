using SCVE.Core;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public abstract class LayoutComponent : Component
    {
        public override void Render(IRenderer renderer)
        {
            foreach (var child in Children)
            {
                child.Render(renderer);
            }
        }
    }
}