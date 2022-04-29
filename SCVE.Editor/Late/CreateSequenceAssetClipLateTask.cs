using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Late
{
    public class CreateSequenceAssetClipLateTask : LateTask
    {
        private readonly SequenceAsset _asset;
        private readonly int _frame;
        private readonly int _track;

        public CreateSequenceAssetClipLateTask(SequenceAsset asset, int frame, int track)
        {
            _asset = asset;
            _frame = frame;
            _track = track;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.OpenedSequence.Tracks[_track].AssetClips.Add(AssetClip.CreateNew(AssetType.Sequence, _asset.Guid, _frame, 20));
        }
    }
}