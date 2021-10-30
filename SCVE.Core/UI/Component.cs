using System;
using System.Collections.Generic;
using SCVE.Core.Misc;
using SCVE.Core.Rendering;

namespace SCVE.Core.UI
{
    public abstract class Component
    {
        protected Component Parent;
        public List<Component> Children = new();

        public float ContentWidth { get; private set; }

        public float ContentHeight { get; private set; }

        /// <summary>
        /// The style of the component
        /// </summary>
        public ComponentStyle Style { get; private set; }

        protected Component() : this(ComponentStyle.Default)
        {
        }

        protected Component(ComponentStyle style)
        {
            Style = style;
            SetContentSize(
                MathF.Max(style.MinWidth, style.Width),
                MathF.Max(style.MinHeight, style.Height)
            );
        }

        public void SetStyle(ComponentStyle style)
        {
            Style = style;
            SetContentSize(
                MathF.Max(MathF.Max(style.MinWidth, style.Width), ContentWidth),
                MathF.Max(MathF.Max(style.MinHeight, style.Height), ContentHeight)
            );
        }

        public void AddChild(Component child)
        {
            Children.Add(child);
            child.Parent = this;
            SubtreeUpdated();
        }

        public void RemoveChild(Component child)
        {
            Children.Remove(child);
            child.Parent = null;
            SubtreeUpdated();
        }

        protected virtual void SubtreeUpdated()
        {
            // Default to update self
            RecalculateSelfContentSize();
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

        protected virtual void SelfProcessUpdate(float deltaTime)
        {
        }

        public void Update(float deltaTime)
        {
            SelfProcessUpdate(deltaTime);
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Update(deltaTime);
            }
        }

        protected void SetContentSize(float width, float height)
        {
            ContentWidth = width;
            ContentHeight = height;
        }

        protected void RenderChildren(IRenderer renderer, float x, float y)
        {
            float offsetX = 0;
            float offsetY = 0;

            switch (Style.AlignmentDirection)
            {
                case AlignmentDirection.Horizontal:
                    switch (Style.HorizontalAlignmentBehavior)
                    {
                        case AlignmentBehavior.Start:
                        {
                            offsetX = x;
                            offsetY = y;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetX += Children[i].ContentWidth;
                            }

                            break;
                        }
                        case AlignmentBehavior.Center:

                            throw new ScveException("Center alignment is currently not supported");

                            break;
                        case AlignmentBehavior.End:

                            throw new ScveException("End alignment is currently not supported");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Style.HorizontalAlignmentBehavior));
                    }

                    break;
                case AlignmentDirection.Vertical:
                    switch (Style.VerticalAlignmentBehavior)
                    {
                        case AlignmentBehavior.Start:
                        {
                            offsetX = x;
                            offsetY = y;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetY += Children[i].ContentHeight;
                            }

                            break;
                        }
                        case AlignmentBehavior.Center:

                            throw new ScveException("Center alignment is currently not supported");

                            break;
                        case AlignmentBehavior.End:

                            throw new ScveException("End alignment is currently not supported");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Style.HorizontalAlignmentBehavior));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RecalculateSelfContentSize()
        {
            float offsetX = 0;
            float offsetY = 0;
            switch (Style.AlignmentDirection)
            {
                case AlignmentDirection.Horizontal:
                    switch (Style.HorizontalAlignmentBehavior)
                    {
                        case AlignmentBehavior.Start:
                        {
                            for (var i = 0; i < Children.Count; i++)
                            {
                                offsetX += Children[i].ContentWidth;
                                offsetY = MathF.Max(offsetY, Children[i].ContentHeight);
                            }

                            break;
                        }
                        case AlignmentBehavior.Center:

                            throw new ScveException("Center alignment is currently not supported");

                            break;
                        case AlignmentBehavior.End:

                            throw new ScveException("End alignment is currently not supported");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Style.HorizontalAlignmentBehavior));
                    }

                    break;
                case AlignmentDirection.Vertical:
                    switch (Style.VerticalAlignmentBehavior)
                    {
                        case AlignmentBehavior.Start:
                        {
                            for (var i = 0; i < Children.Count; i++)
                            {
                                offsetX = MathF.Max(offsetX, Children[i].ContentWidth);
                                offsetY += Children[i].ContentHeight;
                            }

                            break;
                        }
                        case AlignmentBehavior.Center:

                            throw new ScveException("Center alignment is currently not supported");

                            break;
                        case AlignmentBehavior.End:

                            throw new ScveException("End alignment is currently not supported");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Style.HorizontalAlignmentBehavior));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetContentSize(offsetX, offsetY);
        }

        protected abstract void RenderSelf(IRenderer renderer, float x, float y);
    }
}