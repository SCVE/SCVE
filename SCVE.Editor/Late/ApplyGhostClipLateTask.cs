using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.Late
{
    public class ApplyGhostClipLateTask : LateTask
    {
        private GhostClip _ghostClip;

        public ApplyGhostClipLateTask(GhostClip ghostClip)
        {
            _ghostClip = ghostClip;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            // TODO: Check overlap with other clips in current track
            if (_ghostClip.SourceTrackIndex != _ghostClip.CurrentTrackIndex)
            {
                if (_ghostClip.ReferencedClip is EmptyClip empty)
                {
                    visitor.EditingService.OpenedSequence.Tracks[_ghostClip.SourceTrackIndex].EmptyClips.Remove(empty);
                    visitor.EditingService.OpenedSequence.Tracks[_ghostClip.CurrentTrackIndex].EmptyClips.Add(empty);
                }
                else if (_ghostClip.ReferencedClip is AssetClip asset)
                {
                    visitor.EditingService.OpenedSequence.Tracks[_ghostClip.SourceTrackIndex].AssetClips.Remove(asset);
                    visitor.EditingService.OpenedSequence.Tracks[_ghostClip.CurrentTrackIndex].AssetClips.Add(asset);
                }
            }

            if (_ghostClip.CurrentStartFrame != _ghostClip.SourceStartFrame)
            {
                _ghostClip.ReferencedClip!.StartFrame = _ghostClip.CurrentStartFrame;
            }

            if (_ghostClip.CurrentFrameLength != _ghostClip.SourceFrameLength)
            {
                _ghostClip.ReferencedClip!.FrameLength = _ghostClip.CurrentFrameLength;
            }

            _ghostClip.Visible = false;
            _ghostClip.ReferencedClip = null;
            // Console.WriteLine("Applied new states to SourceClip");
        }
    }
}