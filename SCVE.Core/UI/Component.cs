using SCVE.Core.Misc;
using SCVE.Core.Rendering;

namespace SCVE.Core.UI
{
    public abstract class Component
    {
        public Component Parent;

        public float SelfContentWidth { get; protected set; }
        public float SelfContentHeight { get; protected set; }

        public float ScreenWidth { get; set; }
        public float ScreenHeight { get; set; }

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

        public void SetSelfContentSize(float width, float height)
        {
            SelfContentWidth  = width;
            SelfContentHeight = height;
        }

        public void SetScreenSize(float width, float height)
        {
            ScreenWidth  = width;
            ScreenHeight = height;
        }

        public virtual void Reflow(float parentWidth, float parentHeight)
        {
        }

        public abstract void RenderSelf(IRenderer renderer, float x, float y);
    }
}