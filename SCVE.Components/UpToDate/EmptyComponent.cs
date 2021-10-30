using SCVE.Core.Rendering;
using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    public class EmptyComponent : Component
    {
        public EmptyComponent()
        {
        }

        public EmptyComponent(ComponentStyle style) : base(style)
        {
        }

        protected override void RenderSelf(IRenderer renderer, float x, float y)
        {
            RenderChildren(renderer, x, y);
        }
    }
}