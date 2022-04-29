using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Late
{
    public class CreateImageAssetClipLateTask : LateTask
    {
        private readonly ImageAsset _asset;
        private readonly int _frame;
        private readonly int _track;

        public CreateImageAssetClipLateTask(ImageAsset asset, int frame, int track)
        {
            _asset = asset;
            _frame = frame;
            _track = track;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.OpenedSequence.Tracks[_track].AssetClips.Add(AssetClip.CreateNew(AssetType.Image, _asset.Guid, _frame, 20));
        }
    }
}