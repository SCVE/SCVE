using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class FullFlexWhiteRectComponentWhiteRect : RenderableComponent
    {
        public FullFlexWhiteRectComponentWhiteRect()
        {
        }

        public override void Render(IRenderer renderer)
        {
            // renderer.RenderColorRect();

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}