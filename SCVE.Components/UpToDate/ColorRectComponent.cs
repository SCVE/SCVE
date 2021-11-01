using SCVE.Core.Rendering;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class ColorRectComponent : RenderableComponent
    {
        public ColorRectComponent()
        {
            Logger.Construct(nameof(ColorRectComponent));
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            DesiredWidth  = Style.Width.Flatten(availableWidth);
            DesiredHeight = Style.Height.Flatten(availableHeight);
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X = x;
            Y = y;
            
            Width  = Style.Width.Flatten(availableWidth);
            Height = Style.Height.Flatten(availableHeight);
        }

        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent($"{nameof(ColorRectComponent)} {X}:{Y}:{Width}:{Height}:{Style.PrimaryColor}", indent);
        }

        public override void RenderSelf(IRenderer renderer)
        {
            renderer.RenderColorRect(X, Y, Width, Height, Style.PrimaryColor.Value);
        }
    }
}