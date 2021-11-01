using SCVE.Core.Rendering;
using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    public class ClipComponent : ContainerComponent
    {
        public override void RenderSelf(IRenderer renderer)
        {
            // Get the clip rect
            var (x, y, width, height) = renderer.GetClipRect();

            // set from current component
            renderer.SetClipRect(X, Y, Width, Height);
            
            // render
            Component.RenderSelf(renderer);
            
            // restore clip to original
            renderer.SetClipRect(x, y, width, height);
        }
    }
}