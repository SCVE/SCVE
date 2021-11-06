namespace SCVE.UI.Events
{
    public class UIEvent
    {
        public bool Handled { get; set; }

        public Component Target { get; set; }
    }
}