using System;

namespace SCVE.Editor.Late
{
    public class ApplySettingsLateTask : LateTask
    {
        private Settings _settings;

        public ApplySettingsLateTask(Settings settings)
        {
            _settings = settings;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.SettingsService.ApplySettings(_settings);
            Console.WriteLine($"Executed late action: ApplySettings");
        }
    }
}