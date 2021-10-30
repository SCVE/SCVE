using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class FullFlexColoredRectComponent : RenderableComponent
    {
        private ColorRgba _colorRgba;

        public FullFlexColoredRectComponent(ColorRgba colorRgba)
        {
            Logger.Construct(nameof(FullFlexColoredRectComponent));
            _colorRgba = colorRgba;
        }

        public override void Render(IRenderer renderer)
        {
            renderer.RenderColorRect(X, Y, PixelWidth, PixelHeight, _colorRgba);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}