﻿using SCVE.Core.Primitives;
using SCVE.UI.StyleValues;

namespace SCVE.UI
{
    /// <summary>
    /// This class is analogue for CSS styles
    /// </summary>
    public class ComponentStyle
    {
        /// <summary>
        /// Current width of the component
        /// </summary>
        public FloatStyleValue Width { get; set; }

        /// <summary>
        /// Current height of the component
        /// </summary>
        public FloatStyleValue Height { get; set; }

        /// <summary>
        /// Max width of the component, default to <see cref="float.MaxValue">float.MaxValue</see>
        /// </summary>
        public FloatStyleValue MaxWidth { get; set; }

        /// <summary>
        /// Max height of the component, default to <see cref="float.MaxValue">float.MaxValue</see>
        /// </summary>
        public FloatStyleValue MaxHeight { get; set; }

        /// <summary>
        /// Min width of the component, default to 0
        /// </summary>
        public FloatStyleValue MinWidth { get; set; }

        /// <summary>
        /// Min height of the component, default to 0
        /// </summary>
        public FloatStyleValue MinHeight { get; set; }

        /// <summary>
        /// Alignment direction of children
        /// </summary>
        public StyleValue<AlignmentDirection> AlignmentDirection { get; set; }

        /// <summary>
        /// Horizontal alignment of the children, defaults to left
        /// </summary>
        public StyleValue<AlignmentBehavior> HorizontalAlignmentBehavior { get; set; }

        /// <summary>
        /// Vertical alignment of the children, defaults to top
        /// </summary>
        public StyleValue<AlignmentBehavior> VerticalAlignmentBehavior { get; set; }

        /// <summary>
        /// Primary component color, default White
        /// </summary>
        public ColorStyleValue PrimaryColor { get; set; }
        
        public ComponentStyle(FloatStyleValue width, FloatStyleValue height, FloatStyleValue maxWidth, FloatStyleValue maxHeight, FloatStyleValue minWidth, FloatStyleValue minHeight, StyleValue<AlignmentDirection> alignmentDirection, StyleValue<AlignmentBehavior> horizontalAlignmentBehavior, StyleValue<AlignmentBehavior> verticalAlignmentBehavior, ColorStyleValue primaryColor)
        {
            Width                       = width;
            Height                      = height;
            MaxWidth                    = maxWidth;
            MaxHeight                   = maxHeight;
            MinWidth                    = minWidth;
            MinHeight                   = minHeight;
            AlignmentDirection          = alignmentDirection;
            HorizontalAlignmentBehavior = horizontalAlignmentBehavior;
            VerticalAlignmentBehavior   = verticalAlignmentBehavior;
            PrimaryColor                = primaryColor;
        }

        public ComponentStyle(ComponentStyle componentStyle)
        {
            Width                       = componentStyle.Width;
            Height                      = componentStyle.Height;
            MaxWidth                    = componentStyle.MaxWidth;
            MaxHeight                   = componentStyle.MaxHeight;
            MinWidth                    = componentStyle.MinWidth;
            MinHeight                   = componentStyle.MinHeight;
            AlignmentDirection          = componentStyle.AlignmentDirection;
            HorizontalAlignmentBehavior = componentStyle.HorizontalAlignmentBehavior;
            VerticalAlignmentBehavior   = componentStyle.VerticalAlignmentBehavior;
            PrimaryColor                = componentStyle.PrimaryColor;
        }

        public static ComponentStyle Default = new(
            width: new FloatStyleValue(0),
            height: new FloatStyleValue(0),
            maxWidth: new FloatStyleValue(float.MaxValue),
            maxHeight: new FloatStyleValue(float.MaxValue),
            minWidth: new FloatStyleValue(0),
            minHeight: new FloatStyleValue(0),
            alignmentDirection: new StyleValue<AlignmentDirection>(UI.AlignmentDirection.Horizontal),
            horizontalAlignmentBehavior: new StyleValue<AlignmentBehavior>(AlignmentBehavior.Start),
            verticalAlignmentBehavior: new StyleValue<AlignmentBehavior>(AlignmentBehavior.Start),
            primaryColor: new ColorStyleValue(ColorRgba.White)
        );
    }
}