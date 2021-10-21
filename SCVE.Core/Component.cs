using System.Collections.Generic;
using SCVE.Core.Entities;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Core
{
    public abstract class Component : IRenderable
    {
        protected Component Parent;
        protected List<Component> Children = new();

        public ScveMatrix4X4 ModelMatrix = ScveMatrix4X4.Identity;

        public float PixelWidth { get; private set; }
        public float PixelHeight { get; private set; }

        /// <summary>
        /// This should ALWAYS be the most left of the component in screen space
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// This should ALWAYS be the most top of the component in screen space
        /// </summary>
        public float Y { get; private set; }

        // NOTE: This is odd! I can't call OnResize in derived types when it's protected
        protected virtual void OnResized()
        {
            var scale = ScveMatrix4X4.CreateScale(PixelWidth, PixelHeight);
            ModelMatrix.MakeIdentity().Multiply(scale).Multiply(ScveMatrix4X4.CreateTranslation(X, Y));
        }

        public void SetPositionAndSize(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            PixelWidth = width;
            PixelHeight = height;

            OnResized();
        }

        protected Component()
        {
        }

        public virtual void AddChild(Component component)
        {
            Children.Add(component);
            component.SetParent(this);
        }

        public virtual void SetParent(Component component)
        {
            Parent = component;
        }

        public abstract void Render(IRenderer renderer);
    }
}