using SCVE.Core.Rendering;
using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    public class EmptyComponent : Component
    {
        public EmptyComponent()
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