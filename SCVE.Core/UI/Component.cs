using SCVE.Core.Misc;
using SCVE.Core.Rendering;

namespace SCVE.Core.UI
{
    public abstract class Component
    {
        public Component Parent;

        public float X { get; set; }
        public float Y { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float DesiredWidth { get; set; }
        public float DesiredHeight { get; set; }
        
        public virtual bool HasConstMeasure { get; set; }

        /// <summary>
        /// The style of the component
        /// </summary>
        public ComponentStyle Style { get; protected set; }

        protected Component()
        {
        }

        public virtual void OnSetStyle()
        {
        }

        public void SetStyle(ComponentStyle style)
        {
            Style = style;
            OnSetStyle();
        }

        public virtual void AddChild(Component child)
        {
            throw new ScveException($"Unsupported AddChild for component ({GetType().Name})");
        }

        public virtual void RemoveChild(Component child)
        {
            throw new ScveException($"Unsupported RemoveChild for component ({GetType().Name})");
        }

        protected virtual void SubtreeUpdated()
        {
            this.Parent?.SubtreeUpdated();
        }

        /// <summary>
        /// Set a parent component for current component
        /// </summary>
        public void MoveParent(Component parent)
        {
            this.Parent?.RemoveChild(this);

            parent.AddChild(this);
        }

        public virtual void Update(float deltaTime)
        {
        }

        /// <summary>
        /// Measure the desired component size in available space
        /// </summary>
        public virtual void Measure(float availableWidth, float availableHeight)
        {
        }

        /// <summary>
        /// Sets the final size for the current component
        /// </summary>
        public virtual void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X      = x;
            Y      = y;
            Width  = availableWidth;
            Height = availableHeight;
        }

        public abstract void PrintComponentTree(int indent);

        public abstract void RenderSelf(IRenderer renderer);
    }
}