using SCVE.UI.Visitors;

namespace SCVE.UI.Primitive
{
    /// <summary>
    /// FPS Counter is an fps counter (haha). Derivative from Text. It process the update event and displays FPS
    /// </summary>
    public class FpsComponent : TextComponent
    {
        public FpsComponent() : base("arial.ttf", 12, "", TextAlignment.Left)
        {
        }

        public override void Update(float deltaTime)
        {
            SetText($"{1 / deltaTime :F} FPS");
        }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}