using System;
using System.Collections.Generic;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.Groups
{
    /// <summary>
    /// Glue component takes all available space and glues it's children horizontally or vertically 
    /// </summary>
    public class GlueComponent : Component
    {
        public List<Component> Children { get; set; }

        public AlignmentDirection Direction { get; set; }

        public GlueComponent()
        {
            Children = new();
        }

        protected override void SubtreeUpdated()
        {
            if(!Initialized) return;
            Arrange(X, Y, Width, Height);
        }

        public override Component PickComponentByPosition(float x, float y)
        {
            if (x > X && x < X + Width &&
                y > Y && y < Y + Height)
            {
                for (var i = 0; i < Children.Count; i++)
                {
                    var component = Children[i].PickComponentByPosition(x, y);

                    if (component is not null)
                    {
                        return component;
                    }
                }

                return this;
            }
            else
            {
                return null;
            }
        }

        public override void Init()
        {
            base.Init();
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Init();
            }
        }

        public override void AddChild(Component child)
        {
            Children.Add(child);
            child.Parent = this;
            SubtreeUpdated();
        }

        public override void RemoveChild(Component child)
        {
            Children.Remove(child);
            child.Parent = null;
            SubtreeUpdated();
        }

        public override void Update(float deltaTime)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Update(deltaTime);
            }
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            switch (Direction)
            {
                case AlignmentDirection.Horizontal:
                {
                    float usedWidth = 0f;
                    for (var i = 0; i < Children.Count; i++)
                    {
                        Children[i].Measure(availableWidth - usedWidth, availableHeight);
                        usedWidth += Children[i].DesiredWidth;
                        if (usedWidth >= availableWidth)
                        {
                            Logger.Warn($"Glue component exceeded available width! There are {Children.Count - i} children non-present");
                            break;
                        }
                    }

                    break;
                }
                case AlignmentDirection.Vertical:
                {
                    float usedHeight = 0f;
                    for (var i = 0; i < Children.Count; i++)
                    {
                        Children[i].Measure(availableWidth, availableHeight - usedHeight);
                        usedHeight += Children[i].DesiredHeight;
                        if (usedHeight >= availableHeight)
                        {
                            Logger.Warn($"Glue component exceeded available height! There are {Children.Count - i} children non-present");
                            break;
                        }
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DesiredWidth  = availableWidth;
            DesiredHeight = availableHeight;
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            base.Arrange(x, y, availableWidth, availableHeight);

            switch (Direction)
            {
                case AlignmentDirection.Horizontal:
                {
                    float usedWidth = 0f;
                    for (var i = 0; i < Children.Count; i++)
                    {
                        Children[i].Arrange(x + usedWidth, y, availableWidth - usedWidth, availableHeight);
                        usedWidth += Children[i].Width;
                    }

                    break;
                }
                case AlignmentDirection.Vertical:
                {
                    float usedHeight = 0f;
                    for (var i = 0; i < Children.Count; i++)
                    {
                        Children[i].Arrange(x, y + usedHeight, availableWidth, availableHeight - usedHeight);
                        usedHeight += Children[i].Height;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
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