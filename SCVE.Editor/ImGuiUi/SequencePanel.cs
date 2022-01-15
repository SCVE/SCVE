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

        private bool _dragging;

        private GhostClip _ghostClip = new();

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
                var mousePos           = ImGui.GetMousePos();

                var drawOriginX = windowPos.X + contentRegionMin.X;
                var drawOriginY = windowPos.Y + contentRegionMin.Y;

                var windowContentWidth = contentRegionAvail.X;

                var trackHeight = 20;

                var trackMargin = 5;

                var trackHeaderWidth  = 50;
                var trackContentWidth = windowContentWidth - trackHeaderWidth;

                var widthPerFrame = trackContentWidth / EditorApp.Instance.OpenedSequence.FrameLength;

                var sequenceLength = EditorApp.Instance.OpenedSequence.FrameLength;
                for (var i = 0; i < EditorApp.Instance.OpenedSequence.Tracks.Count; i++)
                {
                    var track = EditorApp.Instance.OpenedSequence.Tracks[i];

                    // Track header
                    painter.AddRectFilled(
                        new Vector2(drawOriginX, drawOriginY + i * (trackHeight + trackMargin)),
                        new Vector2(drawOriginX + trackHeaderWidth, drawOriginY + (i + 1) * trackHeight + i * trackMargin),
                        0xFF444444
                    );

                    // track content background
                    painter.AddRectFilled(
                        new Vector2(drawOriginX + trackHeaderWidth, drawOriginY + i * (trackHeight + trackMargin)),
                        new Vector2(drawOriginX + contentRegionAvail.X, drawOriginY + (i + 1) * trackHeight + i * trackMargin),
                        0xFF222222
                    );

                    for (int j = 0; j < EditorApp.Instance.OpenedSequence.Tracks[i].Clips.Count; j++)
                    {
                        var clip = EditorApp.Instance.OpenedSequence.Tracks[i].Clips[j];

                        var clipTopLeft = new Vector2(
                            drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame) / sequenceLength),
                            drawOriginY + (track.Id) * (trackHeight + trackMargin)
                        );
                        var clipBottomRight = new Vector2(
                            drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame + clip.FrameLength) / sequenceLength),
                            drawOriginY + (track.Id + 1) * trackHeight + (track.Id) * trackMargin
                        );

                        _clipRenderer.Render(ref painter, clip, ref clipTopLeft, ref clipBottomRight);

                        ImGui.SetCursorPos(clipTopLeft - windowPos);
                        ImGui.SetItemAllowOverlap();
                        ImGui.InvisibleButton($"##clip{track.Id}{clip.Id}", new Vector2(clipBottomRight.X - clipTopLeft.X, clipBottomRight.Y - clipTopLeft.Y));

                        if (ImGui.IsItemActive())
                        {
                            _draggedClip           = clip;
                            var mouseDragDelta = ImGui.GetMouseDragDelta();
                            
                            _ghostClip.StartFrame  = clip.StartFrame + (int)(mouseDragDelta.X / widthPerFrame);
                            int deltaTracks = (int)(mouseDragDelta.Y/ (trackHeight + trackMargin));
                            
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
                            _ghostClip.Visible     = true;
                            _dragging              = true;
                        }
                    }
                }

                if (_ghostClip.Visible)
                {
                    var clip = _ghostClip;
                    var clipTopLeft = new Vector2(
                        drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame) / sequenceLength),
                        drawOriginY + (_ghostClip.Track.Id) * (trackHeight + trackMargin)
                    );
                    var clipBottomRight = new Vector2(
                        drawOriginX + trackHeaderWidth + trackContentWidth * ((float)(clip.StartFrame + clip.FrameLength) / sequenceLength),
                        drawOriginY + (_ghostClip.Track.Id + 1) * trackHeight + (_ghostClip.Track.Id) * trackMargin
                    );

                    _clipRenderer.RenderGhost(ref painter, clip, ref clipTopLeft, ref clipBottomRight);
                }

                if (!ImGui.IsMouseDown(ImGuiMouseButton.Left))
                {
                    if (_dragging)
                    {
                        // TODO: Check overlap with other clips in current track
                        _draggedClip.StartFrame = _ghostClip.StartFrame;
                        if (_ghostClip.Track != _draggedClip.Track)
                        {
                            _draggedClip.Track.RemoveClip(_draggedClip);
                            _ghostClip.Track.AddClip(_draggedClip);
                            
                        }
                        _ghostClip.Visible      = false;
                    }
                }

                ImGui.End();
            }
        }
    }
}