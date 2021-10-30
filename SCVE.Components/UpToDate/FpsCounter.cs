using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    public class FpsCounter : TextComponent
    {
        public FpsCounter() : this(ComponentStyle.Default)
        {
        }

        public FpsCounter(ComponentStyle style) : base(style, "arial.ttf", 24, "", TextAlignment.Left)
        {
        }

        protected override void SelfProcessUpdate(float deltaTime)
        {
            SetText($"{1 / deltaTime :F} FPS");
        }
    }
}