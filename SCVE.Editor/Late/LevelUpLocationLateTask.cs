using System;

namespace SCVE.Editor.Late
{
    public class LevelUpLocationLateTask : LateTask
    {
        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.ProjectPanelService.LevelUp();
            Console.WriteLine($"Executed late action: LevelUp");
        }
    }
}