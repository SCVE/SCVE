using System;
using System.Collections.Generic;
using SCVE.Core.Rendering;
using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    public class GroupComponent : Component
    {
        public List<Component> Children = new();

        private bool _validReflow = false;

        private void CollectPrimitivesOnly(List<Component> primitives)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                if (Children[i] is GroupComponent group)
                {
                    group.CollectPrimitivesOnly(primitives);
                }
                else
                {
                    primitives.Add(Children[i]);
                }
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

        /// <summary>
        /// Reflows current group and returns true, if current group finished self reflowing
        /// </summary>
        private bool ReflowChildren()
        {
            float contentWidth = 0f;
            float contentHeight = 0f;
            for (var i = 0; i < Children.Count; i++)
            {
                if (Children[i] is GroupComponent { _validReflow: false } group)
                {
                    if (!group.ReflowChildren())
                    {
                        return false;
                    }
                }

                if (Style.AlignmentDirection.Value == AlignmentDirection.Horizontal)
                {
                    contentWidth  += Children[i].SelfContentWidth;
                    contentHeight =  MathF.Max(contentHeight, Children[i].SelfContentHeight);
                }
                else
                {
                    contentWidth  =  MathF.Max(contentWidth, Children[i].SelfContentWidth);
                    contentHeight += Children[i].SelfContentHeight;
                }
            }

            _validReflow = true;
            SetSelfContentSize(contentWidth, contentHeight);

            return true;
        }

        public override void Reflow(float parentWidth, float parentHeight)
        {
            _validReflow = false;

            // If subtree has a fixed size
            if (ReflowChildren())
            {
                // SetSelfContentSize(Style.Width, Style.Height);
            }
            else
            {
                float styleWidth = Style.Width.IsRelative ? parentWidth * Style.Width.Value / 100 : Style.Width.Value;
                float styleMinWidth = Style.MinWidth.IsRelative ? parentWidth * Style.MinWidth.Value / 100 : Style.MinWidth.Value;
                float styleMaxWidth = Style.MaxWidth.IsRelative ? parentWidth * Style.MaxWidth.Value / 100 : Style.MaxWidth.Value;

                float styleHeight = Style.Height.IsRelative ? parentHeight * Style.Height.Value / 100 : Style.Height.Value;
                float styleMinHeight = Style.MinHeight.IsRelative ? parentHeight * Style.MinHeight.Value / 100 : Style.MinHeight.Value;
                float styleMaxHeight = Style.MaxHeight.IsRelative ? parentHeight * Style.MaxHeight.Value / 100 : Style.MaxHeight.Value;

                float constrainedStyleWidth = MathF.Min(MathF.Max(MathF.Max(styleMinWidth, styleWidth), parentWidth), styleMaxWidth);
                float constrainedStyleHeight = MathF.Min(MathF.Max(MathF.Max(styleMinHeight, styleHeight), parentHeight), styleMaxHeight);
                
                SetSelfContentSize(constrainedStyleWidth, constrainedStyleHeight);
            }
        }

        public override void RenderSelf(IRenderer renderer, float x, float y)
        {
            float offsetX = 0;
            float offsetY = 0;

            switch (Style.AlignmentDirection.Value)
            {
                case AlignmentDirection.Horizontal:
                    switch (Style.HorizontalAlignmentBehavior.Value)
                    {
                        case AlignmentBehavior.Start:
                        {
                            offsetX = x;
                            offsetY = y;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetX += Children[i].SelfContentWidth;
                            }

                            break;
                        }
                        case AlignmentBehavior.Center:

                            offsetX = x + SelfContentWidth / 2;
                            offsetY = y;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetX += Children[i].SelfContentWidth;
                            }

                            break;
                        case AlignmentBehavior.End:

                            offsetX = x + Parent.Style.Width - SelfContentWidth;
                            offsetY = y;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetX += Children[i].SelfContentWidth;
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Style.HorizontalAlignmentBehavior));
                    }

                    break;
                case AlignmentDirection.Vertical:
                    switch (Style.VerticalAlignmentBehavior.Value)
                    {
                        case AlignmentBehavior.Start:
                        {
                            offsetX = x;
                            offsetY = y;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetY += Children[i].SelfContentHeight;
                            }

                            break;
                        }
                        case AlignmentBehavior.Center:

                            offsetX = x;
                            offsetY = y + Parent.ScreenHeight / 2;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetY += Children[i].SelfContentHeight;
                            }

                            break;
                        case AlignmentBehavior.End:

                            offsetX = x;
                            offsetY = y + Parent.SelfContentHeight - SelfContentHeight;

                            for (var i = 0; i < Children.Count; i++)
                            {
                                Children[i].RenderSelf(renderer, offsetX, offsetY);
                                offsetY += Children[i].SelfContentHeight;
                            }

                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(Style.HorizontalAlignmentBehavior));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}