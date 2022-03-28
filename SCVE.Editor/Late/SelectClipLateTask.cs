using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.Late
{
    public class SelectClipLateTask : LateTask
    {
        private Clip _clip;

        public SelectClipLateTask(Clip clip)
        {
            _clip = clip;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.SetSelectedClip(_clip);
        }
    }
}