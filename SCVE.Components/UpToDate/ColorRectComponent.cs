using System;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class ColorRectComponent : RenderableComponent
    {
        public ColorRectComponent():base()
        {
            Logger.Construct(nameof(ColorRectComponent));
        }

        public ColorRectComponent(ComponentStyle style) : base(style)
        {
            Logger.Construct(nameof(ColorRectComponent));
        }

        public override void Render(IRenderer renderer, float x, float y)
        {
            float selfWidth = MathF.Max(Style.MinWidth, MathF.Min(Style.MaxWidth, ContentWidth));
            float selfHeight = MathF.Max(Style.MinHeight, MathF.Min(Style.MaxHeight, ContentHeight));

            renderer.RenderColorRect(x, y, selfWidth, selfHeight, Style.PrimaryColor);

            RenderChildren(renderer, x, y);
        }
    }
}