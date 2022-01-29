using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing;
using SCVE.Editor.Modules;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanel : IImGuiRenderable
    {
        private ClipImGuiRenderer _clipRenderer;

        private EditingModule _editingModule;
        private PreviewModule _previewModule;

        public SequencePanel()
        {
            _clipRenderer  = new ClipImGuiRenderer();
            _editingModule = EditorApp.Modules.Get<EditingModule>();
            _previewModule = EditorApp.Modules.Get<PreviewModule>();
        }

        private Clip _draggedClip;

        private bool _isDraggingClip;
        private bool _isDraggingCursor;

        private readonly GhostClip _ghostClip = GhostClip.CreateNew(0, 1);

        private int _cursorDragFrames = 0;

        private static readonly Vector2 _cursorSize = new(10, 20);

        // clockwise order
        private readonly Vector2[] _cursorShapePoints =
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

        private readonly Vector2[] _cursorCurrentPoints = new Vector2[5];

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Sequence Panel"))
            {
                if (_editingModule.OpenedSequence is null)
                {
                    ImGui.Text("No sequence is opened");
                    ImGui.End();
                    return;
                }

                var windowPos = ImGui.GetWindowPos();

                var painter = ImGui.GetWindowDrawList();

                var contentRegionMin = ImGui.GetWindowContentRegionMin();

                var contentRegionAvail = ImGui.GetContentRegionAvail();

                var drawOriginX = windowPos.X + contentRegionMin.X;
                var drawOriginY = windowPos.Y + contentRegionMin.Y;

                var windowContentWidth = contentRegionAvail.X;

                var trackHeight = 20;

                var trackMargin = 5;

                var trackHeaderWidth  = 70;
                var trackContentWidth = windowContentWidth - trackHeaderWidth;

                var sequenceHeaderHeight = 20;

                var timelineFramesMarkerHeight  = 3;
                var timelineSecondsMarkerHeight = 8;

                var widthPerFrame           = trackContentWidth / _editingModule.OpenedSequence.FrameLength;
                var sequenceFPS             = _editingModule.OpenedSequence.FPS;
                var sequenceCursorTimeFrame = _editingModule.OpenedSequence.CursorTimeFrame;

                var sequenceFrameLength = _editingModule.OpenedSequence.FrameLength;

                #region Detect click on timeline (not on cursor)

                ImGui.SetCursorPos(new Vector2(drawOriginX + trackHeaderWidth, drawOriginY) - windowPos);
                ImGui.SetItemAllowOverlap();
                ImGui.InvisibleButton($"##timeline-header", new Vector2(windowContentWidth - trackHeaderWidth, sequenceHeaderHeight));

                if (ImGui.IsItemActive())
                {
                    int timelineClickedFrame = (int)((ImGui.GetMousePos().X - drawOriginX - trackHeaderWidth) / widthPerFrame);
                    if (sequenceCursorTimeFrame != timelineClickedFrame)
                    {
                        _editingModule.OpenedSequence.CursorTimeFrame = Math.Clamp(timelineClickedFrame, 0, sequenceFrameLength);

                        // no sequence data has changed, so we just need to preview new frame
                        _previewModule.RenderCurrentFrame();
                    }
                }

                #endregion

                #region Sequence header

                painter.AddRectFilled(
                    new Vector2(drawOriginX + trackHeaderWidth, drawOriginY),
                    new Vector2(drawOriginX + windowContentWidth, drawOriginY + sequenceHeaderHeight),
                    0xFF333333
                );

                #endregion

                #region Timeline frames markers

                for (int i = 0; i < sequenceFrameLength; i++)
                {
                    int markerStripHeight;
                    if (i % sequenceFPS == 0)
                    {
                        markerStripHeight = timelineSecondsMarkerHeight;
                        var text     = $"{i / sequenceFPS}";
                        var textSize = ImGui.CalcTextSize(text);

                        // seconds text markers
                        painter.AddText(
                            new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame - textSize.X / 2, drawOriginY + ((sequenceHeaderHeight - timelineSecondsMarkerHeight) / 2f) - textSize.Y / 2),
                            0xFFFFFFFF,
                            text
                        );
                    }
                    else
                    {
                        markerStripHeight = timelineFramesMarkerHeight;
                    }

                    painter.AddLine(
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame, drawOriginY + sequenceHeaderHeight - markerStripHeight),
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame, drawOriginY + sequenceHeaderHeight),
                        0xFFFFFFFF
                    );

                    if (_previewModule.HasCached(i))
                    {
                        painter.AddRectFilled(
                            new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame + 1, drawOriginY + sequenceHeaderHeight - markerStripHeight),
                            new Vector2(drawOriginX + trackHeaderWidth + (i + 1) * widthPerFrame - 1, drawOriginY + sequenceHeaderHeight),
                            0xFF00FF00
                        );
                    }
                }

                #endregion

                #region Cursor

                var cursorPosition = new Vector2(drawOriginX + trackHeaderWidth + (sequenceCursorTimeFrame + _cursorDragFrames) * widthPerFrame - _cursorSize.X / 2, drawOriginY);

                ImGui.SetCursorPos(cursorPosition - windowPos);
                ImGui.SetItemAllowOverlap();
                ImGui.InvisibleButton($"##cursor", _cursorSize);

                if (ImGui.IsItemActive())
                {
                    var mouseDragDelta = ImGui.GetMouseDragDelta();
                    _cursorDragFrames = (int)(mouseDragDelta.X / widthPerFrame);
                    _cursorDragFrames = Math.Clamp(_cursorDragFrames, -sequenceCursorTimeFrame, sequenceFrameLength - sequenceCursorTimeFrame);
                    _isDraggingCursor = true;
                }

                for (var i = 0; i < _cursorShapePoints.Length; i++)
                {
                    _cursorCurrentPoints[i].X = _cursorShapePoints[i].X + cursorPosition.X;
                    _cursorCurrentPoints[i].Y = _cursorShapePoints[i].Y + cursorPosition.Y;
                }

                painter.AddConvexPolyFilled(ref _cursorCurrentPoints[0], 5, 0xFFAA6666);

                #endregion

                for (var i = 0; i < _editingModule.OpenedSequence.Tracks.Count; i++)
                {
                    var track = _editingModule.OpenedSequence.Tracks[i];

                    // Track header
                    painter.AddRectFilled(
                        new Vector2(drawOriginX, drawOriginY + sequenceHeaderHeight + i * (trackHeight + trackMargin)),
                        new Vector2(drawOriginX + trackHeaderWidth, drawOriginY + sequenceHeaderHeight + (i + 1) * trackHeight + i * trackMargin),
                        0xFF444444
                    );

                    painter.AddText(new Vector2(drawOriginX, drawOriginY + sequenceHeaderHeight + i * (trackHeight + trackMargin)), 0xFFFFFFFF, $"TRACK {track.Id}");

                    // track content background
                    painter.AddRectFilled(
                        new Vector2(drawOriginX + trackHeaderWidth, drawOriginY + sequenceHeaderHeight + i * (trackHeight + trackMargin)),
                        new Vector2(drawOriginX + contentRegionAvail.X, drawOriginY + sequenceHeaderHeight + (i + 1) * trackHeight + i * trackMargin),
                        0xFF222222
                    );

                    for (int j = 0; j < _editingModule.OpenedSequence.Tracks[i].Clips.Count; j++)
                    {
                        var clip = _editingModule.OpenedSequence.Tracks[i].Clips[j];

                        var clipTopLeft = new Vector2(
                            drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame) / sequenceFrameLength),
                            drawOriginY + sequenceHeaderHeight + (track.Id) * (trackHeight + trackMargin)
                        );
                        var clipBottomRight = new Vector2(
                            drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame + clip.FrameLength) / sequenceFrameLength),
                            drawOriginY + sequenceHeaderHeight + (track.Id + 1) * trackHeight + (track.Id) * trackMargin
                        );

                        _clipRenderer.Render(ref painter, clip, ref clipTopLeft, ref clipBottomRight);

                        ImGui.SetCursorPos(clipTopLeft - windowPos);
                        ImGui.SetItemAllowOverlap();
                        if (ImGui.InvisibleButton($"##clip{clip.Guid:N}", new Vector2(clipBottomRight.X - clipTopLeft.X, clipBottomRight.Y - clipTopLeft.Y)))
                        {
                            _editingModule.SelectedClip = clip;
                        }

                        if (ImGui.IsItemActive())
                        {
                            _draggedClip = clip;
                            var mouseDragDelta = ImGui.GetMouseDragDelta();

                            int deltaTracks = (int)(mouseDragDelta.Y / (trackHeight + trackMargin));

                            int newTrackId = clip.Track.Id + deltaTracks;
                            if (newTrackId >= 0 &&
                                newTrackId < _editingModule.OpenedSequence.Tracks.Count)
                            {
                                _ghostClip.Track = _editingModule.OpenedSequence.Tracks[newTrackId];
                            }
                            else
                            {
                                _ghostClip.Track = clip.Track;
                            }

                            _ghostClip.FrameLength = clip.FrameLength;

                            _ghostClip.StartFrame = clip.StartFrame + (int)(mouseDragDelta.X / widthPerFrame);

                            if (_ghostClip.StartFrame < 0)
                            {
                                _ghostClip.StartFrame = 0;
                            }
                            else if (_ghostClip.EndFrame >= sequenceFrameLength)
                            {
                                _ghostClip.StartFrame = sequenceFrameLength - _ghostClip.FrameLength;
                            }

                            _ghostClip.Visible = true;
                            _isDraggingClip    = true;
                        }
                    }
                }

                if (_ghostClip.Visible)
                {
                    var clip = _ghostClip;
                    var clipTopLeft = new Vector2(
                        drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame) / sequenceFrameLength),
                        drawOriginY + sequenceHeaderHeight + (_ghostClip.Track.Id) * (trackHeight + trackMargin)
                    );
                    var clipBottomRight = new Vector2(
                        drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame + clip.FrameLength) / sequenceFrameLength),
                        drawOriginY + sequenceHeaderHeight + (_ghostClip.Track.Id + 1) * trackHeight + (_ghostClip.Track.Id) * trackMargin
                    );

                    _clipRenderer.RenderGhost(ref painter, clip, ref clipTopLeft, ref clipBottomRight);
                }

                if (_isDraggingClip)
                {
                    if (!ImGui.IsMouseDown(ImGuiMouseButton.Left))
                    {
                        // TODO: Check overlap with other clips in current track
                        if (_ghostClip.Track != _draggedClip.Track)
                        {
                            _draggedClip.Track.RemoveClip(_draggedClip);
                            _ghostClip.Track.AddClip(_draggedClip);
                        }

                        if (_ghostClip.StartFrame != _draggedClip.StartFrame)
                        {
                            _previewModule.InvalidateRange(_draggedClip.StartFrame, _draggedClip.FrameLength);
                            _previewModule.InvalidateRange(_ghostClip.StartFrame, _ghostClip.FrameLength);
                            _draggedClip.StartFrame = _ghostClip.StartFrame;
                        }

                        _ghostClip.Visible      = false;
                        _isDraggingClip         = false;
                    }
                }

                if (_isDraggingCursor)
                {
                    if (!ImGui.IsMouseDown(ImGuiMouseButton.Left))
                    {
                        _editingModule.OpenedSequence.CursorTimeFrame += _cursorDragFrames;
                        _cursorDragFrames                             =  0;
                        _isDraggingCursor                             =  false;

                        _previewModule.RenderCurrentFrame();
                    }
                }

                ImGui.End();
            }
        }
    }
}