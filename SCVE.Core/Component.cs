using System.Collections.Generic;
using SCVE.Core.Entities;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Core
{
    public abstract class Component : IRenderable
    {
        public Rect Rect { get; protected set; }
        
        protected Component Parent;
        protected List<Component> Children = new();

        protected Component(Rect rect)
        {
            Rect = rect;
        }

        public void AddChild(Component component)
        {
            Children.Add(component);
        }

        public void SetParent(Component component)
        {
            Parent = component;
        }

        public abstract void Render(IRenderer renderer);
    }
}