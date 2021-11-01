using System.Collections.Generic;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.UpToDate
{
    /// <summary>
    /// Stack takes all available space and align every child on top of each other
    /// </summary>
    public class StackComponent : Component
    {
        public List<Component> Children;

        public StackComponent()
        {
            Children = new();
        }

        public override void Init()
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Init();
            }
        }

        protected override void SubtreeUpdated()
        {
            Measure(Width, Height);
            Arrange(X, Y, Width, Height);
        }

        public override void AddChild(Component child)
        {
            Children.Add(child);
            child.Parent = this;
            SubtreeUpdated();
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Measure(availableWidth, availableHeight);
            }

            DesiredWidth  = availableWidth;
            DesiredHeight = availableHeight;
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            base.Arrange(x, y, availableWidth, availableHeight);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Arrange(x, y, availableWidth, availableHeight);
            }
        }

        public override void RenderSelf(IRenderer renderer)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].RenderSelf(renderer);
            }
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}