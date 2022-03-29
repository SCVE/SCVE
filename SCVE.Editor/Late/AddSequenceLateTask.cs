using System;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Late
{
    public class AddSequenceLateTask : LateTask
    {
        private SequenceAsset _asset;

        public AddSequenceLateTask(SequenceAsset asset)
        {
            _asset = asset;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.AddSequence(_asset);
            visitor.ProjectPanelService.RescanCurrentLocation();
            Console.WriteLine($"Executed late action: AddSequence");
        }
    }
}