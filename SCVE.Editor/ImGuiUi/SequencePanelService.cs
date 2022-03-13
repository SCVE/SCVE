using System;
using System.ComponentModel;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanelService : IService
    {
        private readonly EditingService _editingService;
        private readonly PreviewService _previewService;

        public Vector2 WindowPosition { get; set; }
        public Vector2 ContentRegionMin { get; set; }
        public Vector2 ContentRegionAvailable { get; set; }
        private int _cursorDragFrames = 0;

        public float DrawOriginX { get; set; }
        public float DrawOriginY { get; set; }
        public float WindowContentWidth { get; set; }

        public readonly int TrackHeight = 20;
        public readonly int TrackMargin = 5;
        public readonly int TrackHeaderWidth = 70;
        public float TrackContentWidth;

        public readonly int SequenceHeaderHeight = 20;

        public readonly int TimelineFramesMarkerHeight = 3;
        public readonly int TimelineSecondsMarkerHeight = 8;

        public float WidthPerFrame;
        public int SequenceFPS;
        public int SequenceCursorTimeFrame;
        public int SequenceFrameLength;

        private bool _isDraggingClip = false;
        private bool _isDraggingCursor = false;

        private readonly Vector2 _cursorSize;

        private readonly Vector2[] _cursorShapePoints;
        private readonly Vector2[] _cursorCurrentPoints;
        private readonly SequencePanelPainter _painter;


        public SequencePanelService(EditingService editingService, PreviewService previewService)
        {
            _editingService = editingService;
            _previewService = previewService;
            // _cursorDragFrames = cursorDragFrames;

            _cursorSize = new(10, 20);
            _cursorShapePoints = new Vector2[]
            {
                // top-left
                new(),
                // top-right
                new(_cursorSize.X, 0),
                // right mid
                new(_cursorSize.X, _cursorSize.Y / 2f),
                // bottom mid
                new(_cursorSize.X / 2, _cursorSize.Y),
                // left mid
                new(0, _cursorSize.Y / 2f),
            };
            _cursorCurrentPoints = new Vector2[5];

            _painter = new SequencePanelPainter(previewService);
        }

        public void RefreshData()
        {
            WindowPosition = ImGui.GetWindowPos();
            ContentRegionMin = ImGui.GetWindowContentRegionMin();
            ContentRegionAvailable = ImGui.GetContentRegionAvail();

            _painter.RefreshPainter();

            DrawOriginX = WindowPosition.X + ContentRegionMin.X;
            DrawOriginY = WindowPosition.Y + ContentRegionMin.Y;
            WindowContentWidth = ContentRegionAvailable.X;

            TrackContentWidth = WindowContentWidth - TrackHeaderWidth;

            WidthPerFrame = TrackContentWidth / _editingService.OpenedSequence.FrameLength;
            SequenceFPS = _editingService.OpenedSequence.FPS;
            SequenceCursorTimeFrame = _editingService.OpenedSequence.CursorTimeFrame;
            SequenceFrameLength = _editingService.OpenedSequence.FrameLength;
        }

        public void DetectClickOnTimeline()
        {
            ImGui.SetCursorPos(new Vector2(DrawOriginX + TrackHeaderWidth, DrawOriginY) - WindowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##timeline-header",
                new Vector2(WindowContentWidth - TrackHeaderWidth, SequenceHeaderHeight));

            if (ImGui.IsItemActive())
            {
                int timelineClickedFrame =
                    (int) ((ImGui.GetMousePos().X - DrawOriginX - TrackHeaderWidth) / WidthPerFrame);
                if (SequenceCursorTimeFrame != timelineClickedFrame)
                {
                    _editingService.OpenedSequence.CursorTimeFrame =
                        Math.Clamp(timelineClickedFrame, 0, SequenceFrameLength);

                    // no sequence data has changed, so we just need to preview new frame
                    _previewService.SyncVisiblePreview();
                }
            }
        }

        public void DrawSequenceHeader()
        {
            _painter.DrawSequenceHeader(
                DrawOriginX,
                DrawOriginY,
                TrackHeaderWidth,
                WindowContentWidth,
                SequenceHeaderHeight
            );
        }

        public void DrawSequenceFramesMarkers()
        {
            _painter.DrawSequenceFramesMarkers(
                SequenceFrameLength,
                SequenceFPS,
                TimelineSecondsMarkerHeight,
                DrawOriginX,
                DrawOriginY,
                TrackHeaderWidth,
                WidthPerFrame,
                SequenceHeaderHeight,
                TimelineFramesMarkerHeight
            );
        }

        public void DrawCursor()
        {
            _painter.DrawCursor(
                DrawOriginX,
                DrawOriginY,
                TrackHeaderWidth,
                SequenceCursorTimeFrame,
                ref _cursorDragFrames,
                WidthPerFrame,
                SequenceFrameLength,
                _cursorSize,
                WindowPosition,
                _cursorShapePoints,
                _cursorCurrentPoints,
                ref _isDraggingCursor
            );
        }

        public void ProcessGhostClip(ClipImGuiRenderer clipRenderer, GhostClip ghostClip)
        {
            if (ghostClip.Visible)
            {
                _painter.ProcessGhostClip(
                    clipRenderer,
                    ghostClip,
                    DrawOriginX,
                    DrawOriginY,
                    TrackHeaderWidth,
                    TrackContentWidth,
                    SequenceFrameLength,
                    SequenceHeaderHeight,
                    TrackMargin,
                    TrackHeight
                );
            }
        }

        public void ProcessDraggingClip(GhostClip ghostClip)
        {
            if (_isDraggingClip)
            {
                if (!ImGui.IsMouseDown(ImGuiMouseButton.Left))
                {
                    // TODO: Check overlap with other clips in current track
                    if (ghostClip.SourceTrackIndex != ghostClip.CurrentTrackIndex)
                    {
                        if (ghostClip.ReferencedClip is EmptyClip empty)
                        {
                            _editingService.OpenedSequence.Tracks[ghostClip.SourceTrackIndex].EmptyClips.Remove(empty);
                            _editingService.OpenedSequence.Tracks[ghostClip.CurrentTrackIndex].EmptyClips.Add(empty);
                        }
                        else if (ghostClip.ReferencedClip is AssetClip asset)
                        {
                            _editingService.OpenedSequence.Tracks[ghostClip.SourceTrackIndex].AssetClips.Remove(asset);
                            _editingService.OpenedSequence.Tracks[ghostClip.CurrentTrackIndex].AssetClips.Add(asset);
                        }
                    }

                    if (ghostClip.StartFrame != ghostClip.SourceStartFrame)
                    {
                        _previewService.InvalidateRange(ghostClip.SourceStartFrame, ghostClip.FrameLength);
                        _previewService.InvalidateRange(ghostClip.StartFrame, ghostClip.FrameLength);
                        ghostClip.ReferencedClip.StartFrame = ghostClip.StartFrame;
                    }

                    ghostClip.Visible = false;
                    ghostClip.ReferencedClip = null!;
                    _isDraggingClip = false;
                }
            }
        }

        public void ProcessDraggingCursor()
        {
            if (_isDraggingCursor)
            {
                if (!ImGui.IsMouseDown(ImGuiMouseButton.Left))
                {
                    _editingService.OpenedSequence.CursorTimeFrame += _cursorDragFrames;
                    _cursorDragFrames = 0;
                    _isDraggingCursor = false;

                    _previewService.SyncVisiblePreview();
                }
            }
        }

        public void DrawTracks(ClipImGuiRenderer clipRenderer, GhostClip ghostClip)
        {
            for (var i = 0; i < _editingService.OpenedSequence.Tracks.Count; i++)
            {
                var track = _editingService.OpenedSequence.Tracks[i];

                // Track header
                _painter.DrawTrackHead(
                    i,
                    DrawOriginX,
                    DrawOriginY,
                    SequenceHeaderHeight,
                    TrackHeight,
                    TrackMargin,
                    TrackHeaderWidth
                );

                // track content background
                _painter.DrawTrackContentBackground(
                    i,
                    DrawOriginX,
                    DrawOriginY,
                    SequenceHeaderHeight,
                    TrackHeight,
                    TrackMargin,
                    TrackHeaderWidth,
                    ContentRegionAvailable
                );

                for (int j = 0; j < _editingService.OpenedSequence.Tracks[i].EmptyClips.Count; j++)
                {
                    var clip = _editingService.OpenedSequence.Tracks[i].EmptyClips[j];

                    _painter.DrawClip(
                        clip,
                        j,
                        clipRenderer,
                        _editingService,
                        DrawOriginX,
                        DrawOriginY,
                        TrackHeaderWidth,
                        TrackContentWidth,
                        SequenceFrameLength,
                        SequenceHeaderHeight,
                        TrackHeight,
                        TrackMargin,
                        WindowPosition
                    );

                    if (ImGui.IsItemActive())
                    {
                        ghostClip.ReferencedClip = clip;
                        ghostClip.SourceTrackIndex = i;
                        ghostClip.SourceStartFrame = clip.StartFrame;
                        var mouseDragDelta = ImGui.GetMouseDragDelta();

                        int deltaTracks = (int) (mouseDragDelta.Y / (TrackHeight + TrackMargin));

                        int newTrackId = i + deltaTracks;
                        if (newTrackId >= 0 &&
                            newTrackId < _editingService.OpenedSequence.Tracks.Count)
                        {
                            ghostClip.CurrentTrackIndex = newTrackId;
                        }
                        else
                        {
                            ghostClip.CurrentTrackIndex = i;
                        }

                        ghostClip.FrameLength = clip.FrameLength;

                        ghostClip.StartFrame = clip.StartFrame + (int) (mouseDragDelta.X / WidthPerFrame);

                        if (ghostClip.StartFrame < 0)
                        {
                            ghostClip.StartFrame = 0;
                        }
                        else if (ghostClip.EndFrame >= SequenceFrameLength)
                        {
                            ghostClip.StartFrame = SequenceFrameLength - ghostClip.FrameLength;
                        }

                        ghostClip.Visible = true;
                        _isDraggingClip = true;
                    }
                }
            }
        }
    }
}