using System;
using SCVE.Core.Misc;
using SCVE.Core.Rendering;
using SCVE.UI.Visitors;

namespace SCVE.UI
{
    public abstract class Component
    {
        public string Id { get; set; }

        public bool Initialized { get; set; }

        public Component Parent;

        public float X { get; set; }
        public float Y { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public float DesiredWidth { get; set; }
        public float DesiredHeight { get; set; }

        public bool IsFocused { get; set; }

        public event Action MouseDown;
        public event Action MouseUp;
        public event Action<float, float> MouseMove;
        public event Action MouseEnter;
        public event Action MouseLeave;
        public event Action Focused;
        public event Action LostFocus;

        /// <summary>
        /// The style of the component
        /// </summary>
        public ComponentStyle Style { get; protected set; }

        protected Component()
        {
        }

        public abstract T FindComponentById<T>(string id) where T : Component;

        public abstract Component PickComponentByPosition(float x, float y);

        public abstract void RenderSelf(IRenderer renderer);


        public virtual void AcceptVisitor(IComponentVisitor visitor)
        {
            throw new ScveException($"Component {GetType().Name} Doesn't implement AcceptVisitor");
        }

        protected virtual void OnSetStyle()
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
            if (!Initialized) return;

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

        public virtual void Init()
        {
            Initialized = true;
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

        public virtual void DispatchMouseDown()
        {
            MouseDown?.Invoke();
        }

        public void DispatchFocus()
        {
            IsFocused = true;
            Focused?.Invoke();
        }


        public void DispatchLostFocus()
        {
            IsFocused = false;
            LostFocus?.Invoke();
        }

        public virtual void DispatchMouseMove(float x, float y)
        {
            MouseMove?.Invoke(x, y);
        }

        public virtual void DispatchMouseUp()
        {
            MouseUp?.Invoke();
        }

        public virtual void DispatchMouseEnter()
        {
            MouseEnter?.Invoke();
        }
        
        public virtual void DispatchMouseLeave()
        {
            MouseLeave?.Invoke();
        }
    }
}