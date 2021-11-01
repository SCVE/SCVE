using System;
using SCVE.Core.Misc;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Core.UI
{
    public abstract class ContainerComponent : Component
    {
        private bool _hasComponent;
        protected Component Component;

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

        public override void Measure(float availableWidth, float availableHeight)
        {
            if (!_hasComponent) return;

            if (!Component.HasConstMeasure)
            {
                Component.Measure(availableWidth, availableHeight);
            }

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

        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent(nameof(ContainerComponent), indent);
            Component.PrintComponentTree(indent + 1);
        }

        public override void RenderSelf(IRenderer renderer)
        {
            if (!_hasComponent) return;
            Component.RenderSelf(renderer);
        }
    }
}