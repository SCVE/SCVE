namespace SCVE.Editor.Late
{
    public abstract class LateTask
    {
        public abstract void AcceptVisitor(LateTaskVisitor visitor);
    }
}