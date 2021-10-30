using SCVE.Core.Misc;
using SCVE.Core.Rendering;
using SCVE.Core.UI;

namespace SCVE.Components.Layouts
{
    public class LayoutCell : Component
    {
        public LayoutCell(Component bootstrapped)
        {
            // Explicitly call add child from base
            base.AddChild(bootstrapped);
        }

        protected override void OnResized()
        {
            // NOTE: Don't need to update any matrices here, so don't call base.OnResized();
            Children[0].SetPositionAndSize(X, Y, PixelWidth, PixelHeight);
        }

        public override void AddChild(Component component)
        {
            throw new ScveException("LayoutCell is supposed to contain only a single child");
        }

        public override void Render(IRenderer renderer)
        {
            Children[0].Render(renderer);
        }
    }
}