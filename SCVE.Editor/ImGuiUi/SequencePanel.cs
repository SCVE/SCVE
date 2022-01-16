using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanel : IImGuiRenderable
    {
        private ClipImGuiRenderer _clipRenderer;

        public SequencePanel()
        {
            _clipRenderer = new ClipImGuiRenderer();
        }

        private Clip _draggedClip;

        private bool _isDraggingClip;
        private bool _isDraggingCursor;

        private GhostClip _ghostClip = GhostClip.CreateNew(0, 1);

        private int _cursorTimeFrame = 0;
        private int _cursorDragFrames = 0;

        private static Vector2 _cursorSize = new Vector2(10, 20);

        // clockwise order
        private Vector2[] _cursorShapePoints =
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

        private Vector2[] _cursorCurrentPoints = new Vector2[5];

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Sequence Panel"))
            {
                if (EditorApp.Instance.OpenedSequence is null)
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

                var widthPerFrame = trackContentWidth / EditorApp.Instance.OpenedSequence.FrameLength;
                var sequenceFPS   = EditorApp.Instance.OpenedSequence.FPS;

                var sequenceFrameLength = EditorApp.Instance.OpenedSequence.FrameLength;

                ImGui.SetCursorPos(new Vector2(drawOriginX + trackHeaderWidth, drawOriginY) - windowPos);
                ImGui.SetItemAllowOverlap();
                ImGui.InvisibleButton($"##timeline-header", new Vector2(windowContentWidth - trackHeaderWidth, sequenceHeaderHeight));

                if (ImGui.IsItemActive())
                {
                    int timelineClickedFrame = (int)((ImGui.GetMousePos().X - drawOriginX - trackHeaderWidth) / widthPerFrame);
                    if (_cursorTimeFrame != timelineClickedFrame)
                    {
                        _cursorTimeFrame = timelineClickedFrame;
                        _cursorTimeFrame = Math.Clamp(_cursorTimeFrame, 0, sequenceFrameLength);
                    }
                }

                // Sequence header
                painter.AddRectFilled(
                    new Vector2(drawOriginX + trackHeaderWidth, drawOriginY),
                    new Vector2(drawOriginX + windowContentWidth, drawOriginY + sequenceHeaderHeight),
                    0xFF333333
                );

                // Timeline frames markers
                for (int i = 0; i < sequenceFrameLength; i++)
                {
                    int height;
                    if (i % sequenceFPS == 0)
                    {
                        height = timelineSecondsMarkerHeight;
                        var text         = $"{i / sequenceFPS}";
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
                        height = timelineFramesMarkerHeight;
                    }

                    painter.AddLine(
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame, drawOriginY + sequenceHeaderHeight - height),
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame, drawOriginY + sequenceHeaderHeight), 0xFFFFFFFF
                    );
                }

                var cursorPosition = new Vector2(drawOriginX + trackHeaderWidth + (_cursorTimeFrame + _cursorDragFrames) * widthPerFrame - _cursorSize.X / 2, drawOriginY);

                ImGui.SetCursorPos(cursorPosition - windowPos);
                ImGui.SetItemAllowOverlap();
                ImGui.InvisibleButton($"##cursor", _cursorSize);

                if (ImGui.IsItemActive())
                {
                    var mouseDragDelta = ImGui.GetMouseDragDelta();
                    _cursorDragFrames = (int)(mouseDragDelta.X / widthPerFrame);
                    _cursorDragFrames = Math.Clamp(_cursorDragFrames, -_cursorTimeFrame, sequenceFrameLength - _cursorTimeFrame);
                    _isDraggingCursor = true;
                }

                for (var i = 0; i < _cursorShapePoints.Length; i++)
                {
                    _cursorCurrentPoints[i].X = _cursorShapePoints[i].X + cursorPosition.X;
                    _cursorCurrentPoints[i].Y = _cursorShapePoints[i].Y + cursorPosition.Y;
                }

                // Cursor
                painter.AddConvexPolyFilled(ref _cursorCurrentPoints[0], 5, 0xFFAA6666);

                for (var i = 0; i < EditorApp.Instance.OpenedSequence.Tracks.Count; i++)
                {
                    var track = EditorApp.Instance.OpenedSequence.Tracks[i];

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

                    for (int j = 0; j < EditorApp.Instance.OpenedSequence.Tracks[i].Clips.Count; j++)
                    {
                        var clip = EditorApp.Instance.OpenedSequence.Tracks[i].Clips[j];

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
                        ImGui.InvisibleButton($"##clip{track.Id}{clip.Id}", new Vector2(clipBottomRight.X - clipTopLeft.X, clipBottomRight.Y - clipTopLeft.Y));

                        if (ImGui.IsItemActive())
                        {
                            _draggedClip = clip;
                            var mouseDragDelta = ImGui.GetMouseDragDelta();

                            int deltaTracks = (int)(mouseDragDelta.Y / (trackHeight + trackMargin));

                            int newTrackId = clip.Track.Id + deltaTracks;
                            if (newTrackId >= 0 &&
                                newTrackId < EditorApp.Instance.OpenedSequence.Tracks.Count)
                            {
                                _ghostClip.Track = EditorApp.Instance.OpenedSequence.Tracks[newTrackId];
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

                        _draggedClip.StartFrame = _ghostClip.StartFrame;
                        _ghostClip.Visible      = false;
                        _isDraggingClip         = false;
                    }
                }

                if (_isDraggingCursor)
                {
                    if (!ImGui.IsMouseDown(ImGuiMouseButton.Left))
                    {
                        _cursorTimeFrame  += _cursorDragFrames;
                        _cursorDragFrames =  0;
                        _isDraggingCursor =  false;
                    }
                }

                ImGui.End();
            }
        }
    }
}