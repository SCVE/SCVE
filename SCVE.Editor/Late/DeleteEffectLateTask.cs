using System;
using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.Late
{
    public class DeleteEffectLateTask : LateTask
    {
        private Clip _clip;
        private int _index;

        public DeleteEffectLateTask(Clip clip, int index)
        {
            _clip = clip;
            _index = index;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            _clip.Effects.RemoveAt(_index);
            Console.WriteLine($"Executed late action: DeleteEffect");
        }
    }
}