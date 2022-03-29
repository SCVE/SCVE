namespace SCVE.Editor.Late
{
    public class MoveCursorLateTask : LateTask
    {
        private int _delta;

        public MoveCursorLateTask(int delta)
        {
            _delta = delta;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.CursorFrame += _delta;
        }
    }
}