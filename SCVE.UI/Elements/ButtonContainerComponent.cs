using SCVE.Core.Misc;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.Elements
{
    public class ButtonContainerComponent : ContainerComponent
    {
        public override void Init()
        {
            base.Init();
            Component.Init();

            var button = FindComponentById<ButtonComponent>("button");

            button.MouseDown  += () => { Logger.Warn("Button Clicked"); };
            button.MouseEnter += () => { button.BackgroundColor = ColorRgba.White; };
            button.MouseLeave += () => { button.BackgroundColor = new ColorRgba(1, 0, 0, 1); };
        }

        public override void AddChild(Component child)
        {
            if (child is ButtonComponent buttonComponent)
            {
                base.AddChild(child);
            }
            else
            {
                throw new ScveException("Not a button passed to button container");
            }
        }

        public override T FindComponentById<T>(string id)
        {
            if (Id == id)
            {
                return this as T;
            }

            return Component.FindComponentById<T>(id);
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            Component.Measure(availableWidth, availableHeight);
            DesiredWidth  = Component.DesiredWidth;
            DesiredHeight = Component.DesiredHeight;
        }

        public override Component PickComponentByPosition(float x, float y)
        {
            return Maths.PointInRect(X, Y, Width, Height, x, y) ? Component.PickComponentByPosition(x, y) : null;
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            Component.Arrange(x, y, availableWidth, availableHeight);
            X      = x;
            Y      = y;
            Width  = Component.Width;
            Height = Component.Height;
        }

        public override void RenderSelf(IRenderer renderer)
        {
            Component.RenderSelf(renderer);
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}