using System.Collections.Generic;
using System.Linq;
using SCVE.Core.Misc;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Visitors;

namespace SCVE.UI.UpToDate
{
    /// <summary>
    /// Flex occupies all available space, positions and resizes child Flex Cells to occupy space based on their Flex property
    /// </summary>
    public class FlexComponent : Component
    {
        private float _totalFlex = 0f;

        public List<FlexCell> Children { get; set; }

        public AlignmentDirection Direction { get; set; }

        public FlexComponent()
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

        public override void AddChild(Component child)
        {
            if (child is FlexCell flexCell)
            {
                Children.Add(flexCell);
                child.Parent =  this;
                _totalFlex   += flexCell.Flex;
            }
            else
            {
                throw new ScveException($"Flex component may not contain any elements except FlexCell. Provided {child.GetType().Name}");
            }
        }

        public override void RemoveChild(Component child)
        {
            if (child is FlexCell flexCell)
            {
                Children.Remove(flexCell);
                child.Parent =  null;
                _totalFlex   -= flexCell.Flex;
            }
            else
            {
                throw new ScveException($"Flex component may not contain any elements except FlexCell. Provided {child.GetType().Name}");
            }
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
            if (Direction == AlignmentDirection.Horizontal)
            {
                float widthPerFlexUnit;
                float heightPerFlexUnit;
                widthPerFlexUnit  = availableWidth / _totalFlex;
                heightPerFlexUnit = availableHeight;
                for (var i = 0; i < Children.Count; i++)
                {
                    var childWidth = Children[i].Flex * widthPerFlexUnit;
                    Children[i].Measure(childWidth, heightPerFlexUnit);
                }
            }
            else if (Direction == AlignmentDirection.Vertical)
            {
                var widthPerFlexUnit = availableWidth;
                var heightPerFlexUnit = availableHeight / _totalFlex;
                for (var i = 0; i < Children.Count; i++)
                {
                    var childHeight = Children[i].Flex * heightPerFlexUnit;
                    Children[i].Measure(widthPerFlexUnit, childHeight);
                }
            }
            else
            {
                throw new ScveException($"Unsupported AlignmentDirection {Direction}");
            }

            DesiredWidth  = availableWidth;
            DesiredHeight = availableHeight;
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            base.Arrange(x, y, availableWidth, availableHeight);

            float offsetX = x;
            float offsetY = y;

            if (Direction == AlignmentDirection.Horizontal)
            {
                float widthPerFlexUnit;
                float heightPerFlexUnit;
                widthPerFlexUnit  = availableWidth / _totalFlex;
                heightPerFlexUnit = availableHeight;
                for (var i = 0; i < Children.Count; i++)
                {
                    var childWidth = Children[i].Flex * widthPerFlexUnit;
                    Children[i].Arrange(offsetX, offsetY, childWidth, heightPerFlexUnit);
                    offsetX += childWidth;
                }
            }
            else if (Direction == AlignmentDirection.Vertical)
            {
                var widthPerFlexUnit = availableWidth;
                var heightPerFlexUnit = availableHeight / _totalFlex;
                for (var i = 0; i < Children.Count; i++)
                {
                    var childHeight = Children[i].Flex * heightPerFlexUnit;
                    Children[i].Arrange(offsetX, offsetY, widthPerFlexUnit, childHeight);
                    offsetY += childHeight;
                }
            }
            else
            {
                throw new ScveException($"Unsupported AlignmentDirection {Direction}");
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