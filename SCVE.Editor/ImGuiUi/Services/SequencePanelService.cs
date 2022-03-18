using System;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Services
{
    public class SequencePanelService : IService
    {
        private readonly EditingService _editingService;
        private readonly PreviewService _previewService;

        private int _cursorDragFrames;

        private bool _isDraggingCursor;

        private readonly SequencePanelPainterService _panelPainterService;

        private readonly GhostClip _ghostClip;

        public SequencePanelService(EditingService editingService, PreviewService previewService, SequencePanelPainterService panelPainterService)
        {
            _editingService = editingService;
            _previewService = previewService;
            _panelPainterService = panelPainterService;

            _ghostClip = new GhostClip();
        }

        public void RefreshData()
        {
            _panelPainterService.SetRenderData(_cursorDragFrames, _editingService.CursorFrame, _editingService.OpenedSequence);
        }

        public void DrawSequenceHeader()
        {
            if (_panelPainterService.DrawSequenceHeader(out var newCursorTimeFrame))
            {
                _editingService.CursorFrame = newCursorTimeFrame;

                // no sequence data has changed, so we just need to preview new frame
                _previewService.SyncVisiblePreview();
            }

            _panelPainterService.DrawSequenceFrameMarkers();
        }

        public void DrawCursor()
        {
            var isDraggingCursorNow = _panelPainterService.DrawCursor(out var newCursorDragFrames);

            if (isDraggingCursorNow)
            {
                _isDraggingCursor = true;
                _cursorDragFrames = newCursorDragFrames;

                _previewService.SyncVisiblePreview(_cursorDragFrames);
            }
            else
            {
                if (_isDraggingCursor)
                {
                    _editingService.CursorFrame += _cursorDragFrames;
                    _cursorDragFrames = 0;
                    _isDraggingCursor = false;

                    _previewService.SyncVisiblePreview();
                }
            }
        }

        public void DrawGhostClip()
        {
            if (_ghostClip.Visible)
            {
                _panelPainterService.DrawGhostClip(_ghostClip);
            }
        }

        public void DrawTracks()
        {
            for (var i = 0; i < _editingService.OpenedSequence.Tracks.Count; i++)
            {
                var track = _editingService.OpenedSequence.Tracks[i];

                // Track header
                _panelPainterService.DrawTrackHead(i);

                // track content background
                _panelPainterService.DrawTrackContentBackground(i);

                for (int j = 0; j < _editingService.OpenedSequence.Tracks[i].EmptyClips.Count; j++)
                {
                    var clip = _editingService.OpenedSequence.Tracks[i].EmptyClips[j];

                    if (_panelPainterService.DrawClip(clip, i, out var isDragging, out var deltaFrames, out var deltaTracks))
                    {
                        _editingService.SetSelectedClip(clip);
                    }

                    if (isDragging)
                    {
                        if (!_ghostClip.Visible)
                        {
                            CreateGhostClip(clip, i);
                        }
                        else
                        {
                            UpdateGhostClipValues(deltaTracks, deltaFrames);
                        }
                    }
                    else
                    {
                        if (_ghostClip.Visible)
                        {
                            ApplyGhostClipValues();
                        }
                    }
                }
            }
        }

        private void UpdateGhostClipValues(int deltaTracks, int deltaFrames)
        {
            if (_ghostClip.CurrentTrackIndex != _ghostClip.SourceTrackIndex + deltaTracks)
            {
                int newTrackIndex = _ghostClip.SourceTrackIndex + deltaTracks;
                if (newTrackIndex >= 0 && newTrackIndex < _editingService.OpenedSequence.Tracks.Count)
                {
                    _ghostClip.CurrentTrackIndex = newTrackIndex;
                    Console.WriteLine($"Updated GhostClip TrackIndex to {newTrackIndex}");
                }
            }

            if (_ghostClip.CurrentStartFrame != _ghostClip.SourceStartFrame + deltaFrames)
            {
                _ghostClip.CurrentStartFrame = _ghostClip.SourceStartFrame + deltaFrames;

                if (_ghostClip.CurrentStartFrame < 0)
                {
                    _ghostClip.CurrentStartFrame = 0;
                }
                else if (_ghostClip.CurrentStartFrame + _ghostClip.CurrentFrameLength >= _editingService.OpenedSequence.FrameLength)
                {
                    _ghostClip.CurrentStartFrame = _editingService.OpenedSequence.FrameLength - _ghostClip.CurrentFrameLength;
                }

                Console.WriteLine($"Updated GhostClip CurrentStartFrame to {_ghostClip.CurrentStartFrame}");
            }
        }

        private void CreateGhostClip(EmptyClip clip, int trackIndex)
        {
            _ghostClip.ReferencedClip = clip;

            _ghostClip.SourceTrackIndex = trackIndex;
            _ghostClip.SourceStartFrame = clip.StartFrame;
            _ghostClip.SourceFrameLength = clip.FrameLength;

            _ghostClip.CurrentTrackIndex = trackIndex;
            _ghostClip.CurrentStartFrame = clip.StartFrame;
            _ghostClip.CurrentFrameLength = clip.FrameLength;

            _ghostClip.Visible = true;
            Console.WriteLine("Created GhostClip");
        }

        private void ApplyGhostClipValues()
        {
            // TODO: Check overlap with other clips in current track
            if (_ghostClip.SourceTrackIndex != _ghostClip.CurrentTrackIndex)
            {
                if (_ghostClip.ReferencedClip is EmptyClip empty)
                {
                    _editingService.OpenedSequence.Tracks[_ghostClip.SourceTrackIndex].EmptyClips.Remove(empty);
                    _editingService.OpenedSequence.Tracks[_ghostClip.CurrentTrackIndex].EmptyClips.Add(empty);
                }
                else if (_ghostClip.ReferencedClip is AssetClip asset)
                {
                    _editingService.OpenedSequence.Tracks[_ghostClip.SourceTrackIndex].AssetClips.Remove(asset);
                    _editingService.OpenedSequence.Tracks[_ghostClip.CurrentTrackIndex].AssetClips.Add(asset);
                }
            }

            if (_ghostClip.CurrentStartFrame != _ghostClip.SourceStartFrame)
            {
                _previewService.InvalidateRange(_ghostClip.SourceStartFrame, _ghostClip.SourceFrameLength);
                _previewService.InvalidateRange(_ghostClip.CurrentStartFrame, _ghostClip.CurrentFrameLength);
                _ghostClip.ReferencedClip!.StartFrame = _ghostClip.CurrentStartFrame;
            }

            _ghostClip.Visible = false;
            _ghostClip.ReferencedClip = null;
            Console.WriteLine("Applied new states to SourceClip");
        }
    }
}