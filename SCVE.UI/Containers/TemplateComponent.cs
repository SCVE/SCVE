using SCVE.Core.Rendering;
using SCVE.UI.Visitors;

namespace SCVE.UI.Containers
{
    public class TemplateComponent : Component
    {
        public string Name { get; set; }

        public Component Component { get; set; }

        public override void Init()
        {
            Component.Init();
        }

        public override Component PickComponentByPosition(float x, float y)
        {
            if (x > X && x < X + Width &&
                y > Y && y < Y + Height)
            {
                var component = Component.PickComponentByPosition(x, y);
                if (component is not null)
                {
                    return component;
                }

                return this;
            }
            else
            {
                return null;
            }
        }

        public override void Update(float deltaTime)
        {
            Component.Update(deltaTime);
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            Component.Measure(availableWidth, availableHeight);
            DesiredWidth  = Component.Width;
            DesiredHeight = Component.Height;
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
    }
}