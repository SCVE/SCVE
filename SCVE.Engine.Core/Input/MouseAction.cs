namespace SCVE.Engine.Core.Input
{
    public class MouseAction
    {
        public MouseCode Button { get; set; }

        public InputActionType Type { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public MouseAction(MouseCode button, InputActionType type, float x, float y)
        {
            Button = button;
            Type = type;
            X = x;
            Y = y;
        }
    }
}