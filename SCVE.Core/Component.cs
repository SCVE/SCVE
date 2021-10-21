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

        public float PixelWidth { get; set; }
        public float PixelHeight { get; set; }

        /// <summary>
        /// This should ALWAYS be the most left of the component
        /// </summary>
        public float X { get; set; }
        
        /// <summary>
        /// This should ALWAYS be the most top of the component
        /// </summary>
        public float Y { get; set; }

        // NOTE: This is odd! I can't call OnResize in derived types when it's protected
        public virtual void OnResize()
        {
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