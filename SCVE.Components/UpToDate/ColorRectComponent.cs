using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class ColorRectComponent : RenderableComponent
    {
        private ColorRgba _colorRgba;

        public ColorRectComponent(ColorRgba colorRgba)
        {
            Logger.Construct(nameof(ColorRectComponent));
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