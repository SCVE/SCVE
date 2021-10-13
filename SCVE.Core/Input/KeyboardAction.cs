namespace SCVE.Core.Input
{
    public class KeyboardAction
    {
        public KeyCode Key { get; set; }
        public InputActionType Type { get; set; }

        public KeyboardAction(KeyCode key, InputActionType type)
        {
            Key = key;
            Type = type;
        }
    }
}