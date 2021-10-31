using SCVE.Core.UI;

namespace SCVE.Components.UpToDate
{
    public class FpsCounter : TextComponent
    {
        public FpsCounter() : base("arial.ttf", 24, "", TextAlignment.Left)
        {
        }

        public override void Update(float deltaTime)
        {
            SetText($"{1 / deltaTime :F} FPS");
        }
    }
}