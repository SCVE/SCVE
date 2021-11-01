namespace SCVE.UI.UpToDate
{
    /// <summary>
    /// FPS Counter is an fps counter (haha). Derivative from Text. It process the update event and displays FPS
    /// </summary>
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