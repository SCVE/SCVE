using System;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Effects;

namespace SCVE.Editor.Late
{
    public class AddEffectLateTask : LateTask
    {
        private Clip _clip;
        private EffectBase _effect;

        public AddEffectLateTask(Clip clip, EffectBase effect)
        {
            _clip = clip;
            _effect = effect;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            _clip.Effects.Add(_effect);
            Console.WriteLine($"Executed late action: AddEffect");
        }
    }
}