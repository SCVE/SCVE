namespace SCVE.Editor.Late
{
    public class TeleportCursorLateTask : LateTask
    {
        private int _frame;

        public TeleportCursorLateTask(int frame)
        {
            _frame = frame;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.CursorFrame = _frame;
        }
    }
}