using System;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.ImGuiUi.Models;
using SCVE.Editor.Late;
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

        public SequencePanelService(EditingService editingService, PreviewService previewService,
            SequencePanelPainterService panelPainterService)
        {
            _editingService = editingService;
            _previewService = previewService;
            _panelPainterService = panelPainterService;

            _ghostClip = new GhostClip();
        }

        public void RefreshData()
        {
            _panelPainterService.SetRenderData(_cursorDragFrames, _editingService.CursorFrame,
                _editingService.OpenedSequence);
        }

        public void DrawSequenceHeader()
        {
            if (_panelPainterService.DrawSequenceHeader(out var newCursorTimeFrame))
            {
                EditorApp.Late(new TeleportCursorLateTask(newCursorTimeFrame));
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
            }
            else
            {
                if (_isDraggingCursor)
                {
                    EditorApp.Late(new MoveCursorLateTask(_cursorDragFrames));
                    _cursorDragFrames = 0;
                    _isDraggingCursor = false;
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

                    DrawClipProcessManipulation(clip, i);
                }
                for (int j = 0; j < _editingService.OpenedSequence.Tracks[i].AssetClips.Count; j++)
                {
                    var clip = _editingService.OpenedSequence.Tracks[i].AssetClips[j];

                    DrawClipProcessManipulation(clip, i);
                }
            }
        }

        private void DrawClipProcessManipulation(Clip clip, int trackIndex)
        {
            var clipManipulationData = new ClipManipulationData();
            _panelPainterService.DrawClip(clip, trackIndex, ref clipManipulationData);

            if (clipManipulationData.IsAnyPartClicked())
            {
                EditorApp.Late(new SelectClipLateTask(clip));
            }

            if (clipManipulationData.IsAnyPartActive() && !_ghostClip.Visible)
            {
                CreateGhostClip(clip, trackIndex);
            }

            if (clipManipulationData.IsBodyActive)
            {
                UpdateGhostClipValues(clipManipulationData.DeltaTracks, clipManipulationData.BodyDragDeltaFrames);
            }
            else if (clipManipulationData.IsHeadActive)
            {
                _ghostClip.CurrentStartFrame = _ghostClip.SourceStartFrame + clipManipulationData.HeadDragDeltaFrames;
                _ghostClip.CurrentFrameLength = _ghostClip.SourceFrameLength - clipManipulationData.HeadDragDeltaFrames;
            }
            else if (clipManipulationData.IsTailActive)
            {
                _ghostClip.CurrentFrameLength = _ghostClip.SourceFrameLength + clipManipulationData.TailDragDeltaFrames;
            }
            else
            {
                if (_ghostClip.Visible)
                {
                    ApplyGhostClipValues();
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

        private void CreateGhostClip(Clip clip, int trackIndex)
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
            EditorApp.Late(new ApplyGhostClipLateTask(_ghostClip));
        }
    }
}