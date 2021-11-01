using SCVE.Core.App;
using SCVE.Core.Lifecycle;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.UI
{
    public class BootstrapableComponent : ContainerComponent, IBootstrapable
    {
        public BootstrapableComponent(Component bootstrappedComponent)
        {
            AddChild(bootstrappedComponent);
        }

        public void Init()
        {
            Reflow();

            Application.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
        }

        public override void Update(float deltaTime)
        {
            Component.Update(deltaTime);
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
            Logger.WarnIndent(nameof(BootstrapableComponent), indent);
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