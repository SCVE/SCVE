using System;

namespace SCVE.Core.UI
{
    public class Reflower
    {
        public static void Reflow(Component component)
        {
        }

        public static (float width, float height) CalculateContentSizeRecursive(Component component)
        {
            throw new NotImplementedException();
            // float offsetX = 0;
            // float offsetY = 0;
            //
            // switch (component.Style.AlignmentDirection.Value)
            // {
            //     case AlignmentDirection.Horizontal:
            //
            //         for (var i = 0; i < component.Children.Count; i++)
            //         {
            //             offsetX += component.Children[i].ContentWidth;
            //             offsetY =  MathF.Max(offsetY, component.Children[i].ContentHeight);
            //         }
            //
            //         break;
            //     case AlignmentDirection.Vertical:
            //
            //         for (var i = 0; i < component.Children.Count; i++)
            //         {
            //             offsetX =  MathF.Max(offsetX, component.Children[i].ContentWidth);
            //             offsetY += component.Children[i].ContentHeight;
            //         }
            //
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
            //
            // return (offsetX, offsetY);
        }
    }
}