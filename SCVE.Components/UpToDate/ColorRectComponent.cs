using System;
using SCVE.Core.Primitives;
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

        public override void OnSetStyle()
        {
            SetSelfContentSize(Style.Width, Style.Height);
            Logger.Warn($"Set ColorRect Color to {Style.PrimaryColor.Value}");
        }

        public override void RenderSelf(IRenderer renderer, float x, float y)
        {
            renderer.RenderColorRect(x, y, SelfContentWidth, SelfContentHeight, Style.PrimaryColor.Value);
        }
    }
}