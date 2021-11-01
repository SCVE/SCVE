using SCVE.Core.Rendering;
using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    /// <summary>
    /// Clip Component fills all available space and clips any rendering of it's child to the occupying space
    /// </summary>
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