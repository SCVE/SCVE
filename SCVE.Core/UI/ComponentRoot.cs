using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Core.UI
{
    public class ComponentRoot : Component, IRenderable
    {
        private Component _bootstrappedComponent;

        public ComponentRoot(Component bootstrappedComponent)
        {
            _bootstrappedComponent       = bootstrappedComponent;
            bootstrappedComponent.Parent = this;

            ScreenWidth  = Application.Instance.MainWindow.Width;
            ScreenHeight = Application.Instance.MainWindow.Height;

            Reflow(ScreenWidth, ScreenHeight);
            Application.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
        }

        private void InputOnWindowSizeChanged(int arg1, int arg2)
        {
            ScreenWidth  = (float)Application.Instance.MainWindow.Width;
            ScreenHeight = (float)Application.Instance.MainWindow.Height;
            Reflow(ScreenWidth, ScreenHeight);
        }

        protected override void SubtreeUpdated()
        {
            Logger.Warn("Component Root Subtree Updated");
            Reflow(ScreenWidth, ScreenHeight);
        }

        public override void Reflow(float parentWidth, float parentHeight)
        {
            _bootstrappedComponent.Reflow(parentWidth, parentHeight);
        }

        public override void RenderSelf(IRenderer renderer, float x, float y)
        {
            _bootstrappedComponent.RenderSelf(renderer, x, y);
        }

        public void Render(IRenderer renderer)
        {
            RenderSelf(renderer, 0, 0);
        }
    }
}