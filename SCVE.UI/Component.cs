using SCVE.Core.Input;
using SCVE.Core.Misc;
using SCVE.Core.Rendering;
using SCVE.UI.Events;
using SCVE.UI.Visitors;

namespace SCVE.UI
{
    public abstract class Component
    {
        public int Id { get; set; }

        public Component Parent;

        public float X { get; set; }
        public float Y { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float DesiredWidth { get; set; }
        public float DesiredHeight { get; set; }

        /// <summary>
        /// The style of the component
        /// </summary>
        public ComponentStyle Style { get; protected set; }

        protected Component()
        {
        }

        public virtual void AcceptVisitor(IComponentVisitor visitor)
        {
            throw new ScveException($"Component {GetType().Name} Doesn't implement AcceptVisitor");
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

        /// <summary>
        /// Bubbles up the event, when a subtree of current component was updated
        /// </summary>
        protected virtual void SubtreeUpdated()
        {
            this.Parent?.SubtreeUpdated();
        }

        public abstract Component PickComponentByPosition(float x, float y);

        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        /// Down to top event bubbling
        /// </summary>
        // public abstract void BubbleEvent<T>(T ev) where T : UIEvent;

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

        public virtual void Init()
        {
        }

        /// <summary>
        /// Measure the desired component size in available space
        /// </summary>
        public virtual void Measure(float availableWidth, float availableHeight)
        {
        }

        /// <summary>
        /// Applies given x, y, width and height to the current component. Does not affect any children by default!
        /// </summary>
        public virtual void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X      = x;
            Y      = y;
            Width  = availableWidth;
            Height = availableHeight;
        }

        public abstract void RenderSelf(IRenderer renderer);

        public virtual void BubbleEvent(string name)
        {
        }
    }
}