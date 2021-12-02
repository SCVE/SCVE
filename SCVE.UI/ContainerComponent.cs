using SCVE.Core.Misc;
using SCVE.Core.Rendering;

namespace SCVE.UI
{
    /// <summary>
    /// Container component contains a single component. occupies all available space
    /// </summary>
    public abstract class ContainerComponent : Component
    {
        private bool _hasComponent;
        public Component Component;

        /// <summary>
        /// Initializes the contained component
        /// </summary>
        public override void Init()
        {
            base.Init();
            Component.Init();
        }

        public override T FindComponentById<T>(string id)
        {
            if (Id == id)
            {
                if (typeof(T) != this.GetType())
                {
                    return null;
                }
                return this as T;
            }

            return Component.FindComponentById<T>(id);
        }

        public override void AddChild(Component child)
        {
            if (_hasComponent)
            {
                throw new ScveException("Can't add a child to container, because there is an existing one");
            }

            Component     = child;
            child.Parent  = this;
            _hasComponent = true;
        }

        public override void RemoveChild(Component child)
        {
            if (!_hasComponent)
            {
                throw new ScveException("Can't remove a child from container, because there is no existing one");
            }

            if (Component != child)
            {
                throw new ScveException("Can't remove a child from component, because it's not it's child");
            }

            Component     = null;
            child.Parent  = null;
            _hasComponent = false;
        }

        /// <summary>
        /// Measures the contained component as-is, occupies all available space
        /// </summary>
        public override void Measure(float availableWidth, float availableHeight)
        {
            if (!_hasComponent) return;

            Component.Measure(availableWidth, availableHeight);

            // By default, any container wants all the available space
            DesiredWidth  = availableWidth;
            DesiredHeight = availableHeight;
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            if (!_hasComponent) return;

            base.Arrange(x, y, availableWidth, availableHeight);

            Component.Arrange(x, y, Width, Height);
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
                else
                {
                    return this;
                }
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

        public override void RenderSelf(IRenderer renderer)
        {
            if (!_hasComponent) return;
            Component.RenderSelf(renderer);
        }
    }
}