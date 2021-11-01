using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Core.UI
{
    public class ComponentRoot : ContainerComponent, IRenderable
    {
        public ComponentRoot(Component bootstrappedComponent)
        {
            AddChild(bootstrappedComponent);
            
            Reflow();

            Application.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
        }

        private void InputOnWindowSizeChanged(int arg1, int arg2)
        {
            Reflow();
        }

        protected override void SubtreeUpdated()
        {
            Logger.Warn("Component Root Subtree Updated");

            Component.Measure(DesiredWidth, DesiredHeight);
            Component.Arrange(0, 0, DesiredWidth, DesiredHeight);
        }

        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent(nameof(ComponentRoot), indent);
            Component.PrintComponentTree(indent + 1);
        }

        public void Reflow()
        {
            DesiredWidth  = Application.Instance.MainWindow.Width;
            DesiredHeight = Application.Instance.MainWindow.Height;

            if (!Component.HasConstMeasure)
            {
                Component.Measure(DesiredWidth, DesiredHeight);
            }

            Component.Arrange(0, 0, DesiredWidth, DesiredHeight);
        }

        public void Render(IRenderer renderer)
        {
            RenderSelf(renderer);
        }
    }
}