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

        private readonly ClipManipulationData _clipManipulationData;

        public SequencePanelService(EditingService editingService, PreviewService previewService,
            SequencePanelPainterService panelPainterService)
        {
            _editingService = editingService;
            _previewService = previewService;
            _panelPainterService = panelPainterService;

            _clipManipulationData = new();

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

                // EditorApp.Late("sync preview", () => { _previewService.SyncVisiblePreview(_cursorDragFrames); });
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
                    _panelPainterService.DrawClip(clip, i, _clipManipulationData);

                    if (_clipManipulationData.IsAnyPartClicked())
                    {
                        EditorApp.Late(new SelectClipLateTask(clip));
                    }

                    if (_clipManipulationData.IsBodyActive)
                    {
                        if (!_ghostClip.Visible)
                        {
                            CreateGhostClip(clip, i);
                        }
                        else
                        {
                            UpdateGhostClipValues(_clipManipulationData.DeltaTracks,
                                _clipManipulationData.BodyDragDeltaFrames);
                        }
                    }
                    else if (_clipManipulationData.IsLeftActive)
                    {
                        if (!_ghostClip.Visible)
                        {
                            CreateGhostClip(clip, i);
                        }
                        else
                        {
                            _ghostClip.CurrentStartFrame =
                                _ghostClip.SourceStartFrame + _clipManipulationData.LeftDragDeltaFrames;
                            _ghostClip.CurrentFrameLength =
                                _ghostClip.SourceFrameLength - _clipManipulationData.LeftDragDeltaFrames;
                        }
                    }
                    else if (_clipManipulationData.IsRightActive)
                    {
                        if (!_ghostClip.Visible)
                        {
                            CreateGhostClip(clip, i);
                        }
                        else
                        {
                            _ghostClip.CurrentFrameLength =
                                _ghostClip.SourceFrameLength + _clipManipulationData.RightDragDeltaFrames;
                        }
                    }
                    else
                    {
                        if (_ghostClip.Visible)
                        {
                            ApplyGhostClipValues();
                        }
                    }
                    
                    _clipManipulationData.Reset();
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
                else if (_ghostClip.CurrentStartFrame + _ghostClip.CurrentFrameLength >=
                         _editingService.OpenedSequence.FrameLength)
                {
                    _ghostClip.CurrentStartFrame =
                        _editingService.OpenedSequence.FrameLength - _ghostClip.CurrentFrameLength;
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
            EditorApp.Late(new ApplyGhostClipLateTask(_ghostClip));
        }
    }
}