using SCVE.Core.App;
using SCVE.Core.Rendering;

namespace SCVE.Core.UI
{
    public class ComponentRoot : Component
    {
        public ComponentRoot(Component bootstrappedComponent)
        {
            AddChild(bootstrappedComponent);
            bootstrappedComponent.MoveParent(this);

            this.Style.Width = Application.Instance.MainWindow.Width;
            this.Style.Height = Application.Instance.MainWindow.Height;
        }

        private void BootstrappedComponentContentSizeChanged()
        {
            // TODO: Propagate event back to update width and height
        }

        public override void Render(IRenderer renderer, float x, float y)
        {
            RenderChildren(renderer, x, y);
        }
    }
}