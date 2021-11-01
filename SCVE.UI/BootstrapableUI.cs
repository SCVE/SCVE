using SCVE.Core.App;
using SCVE.Core.Lifecycle;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI
{
    public class BootstrapableUI : ContainerComponent, IBootstrapable
    {
        public BootstrapableUI()
        {
        }

        public BootstrapableUI WithBootstraped(Component bootstrappedComponent)
        {
            AddChild(bootstrappedComponent);
            return this;
        }

        public override void Init()
        {
            base.Init();

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
            Logger.Warn($"{nameof(BootstrapableUI)} Subtree Updated");

            Component.Measure(DesiredWidth, DesiredHeight);
            Component.Arrange(0, 0, DesiredWidth, DesiredHeight);
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

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}