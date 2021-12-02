using SCVE.Core.Input;
using SCVE.Core.Lifecycle;
using SCVE.Core.Main;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI
{
    public class EngineRunnableUI : ContainerComponent, IEngineRunnable
    {
        private Component _lastMouseOverComponent;
        private Component _focusedComponent;

        public EngineRunnableUI()
        {
        }

        public EngineRunnableUI WithBootstraped(Component bootstrappedComponent)
        {
            AddChild(bootstrappedComponent);
            return this;
        }

        public override void Init()
        {
            base.Init();

            Reflow();

            Engine.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
            Engine.Instance.Input.CursorMoved       += InputOnCursorMoved;
            Engine.Instance.Input.MouseButtonDown   += InputOnMouseButtonDown;
            Engine.Instance.Input.MouseButtonUp     += InputOnMouseButtonUp;
        }

        private void InputOnMouseButtonUp(MouseCode code)
        {
            if (_lastMouseOverComponent is not null)
            {
                _lastMouseOverComponent.DispatchMouseUp();
            }
            else
            {
                Logger.Warn("Mouse is not over any component");
            }
        }

        private void InputOnMouseButtonDown(MouseCode code)
        {
            if (_focusedComponent is not null && _focusedComponent != _lastMouseOverComponent)
            {
                _focusedComponent.DispatchLostFocus();
                
                _lastMouseOverComponent.DispatchFocus();
                _focusedComponent = _lastMouseOverComponent;
            }

            if (_lastMouseOverComponent is not null)
            {
                _lastMouseOverComponent.DispatchMouseDown();
            }
            else
            {
                Logger.Warn("Mouse is not over any component");
            }
        }

        private void InputOnCursorMoved(float x, float y)
        {
            var nowMouseOverComponent = PickComponentByPosition(x, y);
            if (nowMouseOverComponent is null)
            {
                return;
            }
            if (_focusedComponent is not null)
            {
                if (_focusedComponent == nowMouseOverComponent)
                {
                    _focusedComponent.DispatchMouseMove(x, y);
                    Logger.Warn($"Cursor moved to {x}-{y}. Over focused {_lastMouseOverComponent?.GetType().Name}");
                }
                else
                {
                    
                }
            }
            else
            {
                if (nowMouseOverComponent != _lastMouseOverComponent)
                {
                    _lastMouseOverComponent?.DispatchMouseLeave();
                    nowMouseOverComponent.DispatchMouseEnter();
                    Logger.Warn($"Cursor moved to {x}-{y}. Now over {nowMouseOverComponent?.GetType().Name}");
                }
                else
                {
                    nowMouseOverComponent.DispatchMouseMove(x, y);
                }
                _lastMouseOverComponent = nowMouseOverComponent;
            }
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
            Logger.Warn($"{nameof(EngineRunnableUI)} Subtree Updated");

            Component.Measure(DesiredWidth, DesiredHeight);
            Component.Arrange(0, 0, DesiredWidth, DesiredHeight);
        }

        public void Reflow()
        {
            DesiredWidth  = Engine.Instance.MainWindow.Width;
            DesiredHeight = Engine.Instance.MainWindow.Height;

            Component.Measure(DesiredWidth, DesiredHeight);

            Component.Arrange(0, 0, DesiredWidth, DesiredHeight);

            base.Arrange(0, 0, DesiredWidth, DesiredHeight);
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