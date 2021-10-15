using SCVE.Core;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class EmptyComponent : Component
    {
        public EmptyComponent(Rect rect) : base(rect)
        {
        }

        public override void Render(IRenderer renderer)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}