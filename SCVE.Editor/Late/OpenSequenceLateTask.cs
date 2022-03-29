using System;
using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.Late
{
    public class OpenSequenceLateTask : LateTask
    {
        private Sequence _sequence;

        public OpenSequenceLateTask(Sequence sequence)
        {
            _sequence = sequence;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.SetOpenedSequence(_sequence);
            visitor.PreviewService.SwitchToSequence(visitor.EditingService.OpenedSequence);
            Console.WriteLine($"Executed late action: OpenSequence");
        }
    }
}