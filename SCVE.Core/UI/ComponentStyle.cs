using SCVE.Core.Primitives;

namespace SCVE.Core.UI
{
    /// <summary>
    /// This class is analogue for CSS styles
    /// </summary>
    public class ComponentStyle
    {
        /// <summary>
        /// Current width of the component
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Current height of the component
        /// </summary>
        public float Height { get; set; }
        
        /// <summary>
        /// Max width of the component, default to <see cref="float.MaxValue">float.MaxValue</see>
        /// </summary>
        public float MaxWidth { get; set; }

        /// <summary>
        /// Max height of the component, default to <see cref="float.MaxValue">float.MaxValue</see>
        /// </summary>
        public float MaxHeight { get; set; }

        /// <summary>
        /// Min width of the component, default to 0
        /// </summary>
        public float MinWidth { get; set; }

        /// <summary>
        /// Min height of the component, default to 0
        /// </summary>
        public float MinHeight { get; set; }

        /// <summary>
        /// Alignment direction of children
        /// </summary>
        public AlignmentDirection AlignmentDirection { get; set; }

        /// <summary>
        /// Horizontal alignment of the children, defaults to left
        /// </summary>
        public AlignmentBehavior HorizontalAlignmentBehavior { get; set; }

        /// <summary>
        /// Vertical alignment of the children, defaults to top
        /// </summary>
        public AlignmentBehavior VerticalAlignmentBehavior { get; set; }

        /// <summary>
        /// Primary component color, default White
        /// </summary>
        public ColorRgba PrimaryColor { get; set; }

        public ComponentStyle(float width, float height, float maxWidth, float maxHeight, float minWidth, float minHeight, AlignmentDirection alignmentDirection, AlignmentBehavior horizontalAlignmentBehavior, AlignmentBehavior verticalAlignmentBehavior, ColorRgba primaryColor)
        {
            Width = width;
            Height = height;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
            MinWidth = minWidth;
            MinHeight = minHeight;
            AlignmentDirection = alignmentDirection;
            HorizontalAlignmentBehavior = horizontalAlignmentBehavior;
            VerticalAlignmentBehavior = verticalAlignmentBehavior;
            PrimaryColor = primaryColor;
        }

        public static ComponentStyle Default = new(
            width: 0,
            height: 0,
            maxWidth: float.MaxValue,
            maxHeight: float.MaxValue,
            minWidth: 0,
            minHeight: 0,
            alignmentDirection: AlignmentDirection.Horizontal,
            horizontalAlignmentBehavior: AlignmentBehavior.Start,
            verticalAlignmentBehavior: AlignmentBehavior.Start,
            primaryColor: ColorRgba.White
        );
    }
}