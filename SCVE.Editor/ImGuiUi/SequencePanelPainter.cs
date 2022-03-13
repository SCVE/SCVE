using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Imaging;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanelPainter
    {
        private ImDrawListPtr _painter;

        private readonly PreviewService _previewService;

        const int TrackMarginLeft = 10;

        public SequencePanelPainter(PreviewService previewService)
        {
            _previewService = previewService;
            _painter = ImGui.GetWindowDrawList();
        }


        public void DrawSequenceHeader(
            float drawOriginX,
            float drawOriginY,
            float trackHeaderWidth,
            float windowContentWidth,
            float sequenceHeaderHeight
        )
        {
            _painter.AddRectFilled(
                new Vector2(drawOriginX + trackHeaderWidth, drawOriginY),
                new Vector2(drawOriginX + windowContentWidth, drawOriginY + sequenceHeaderHeight),
                0xFF333333
            );
        }

        public void DrawSequenceFramesMarkers(
            int sequenceFrameLength,
            int sequenceFPS,
            int timelineSecondsMarkerHeight,
            float drawOriginX,
            float drawOriginY,
            float trackHeaderWidth,
            float widthPerFrame,
            int sequenceHeaderHeight,
            int timelineFramesMarkerHeight
        )
        {
            for (int i = 0; i < sequenceFrameLength; i++)
            {
                int markerStripHeight;
                if (i % sequenceFPS == 0)
                {
                    markerStripHeight = timelineSecondsMarkerHeight;
                    var text = $"{i / sequenceFPS}";
                    var textSize = ImGui.CalcTextSize(text);

                    // seconds text markers
                    _painter.AddText(
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame - textSize.X / 2,
                            drawOriginY + ((sequenceHeaderHeight - timelineSecondsMarkerHeight) / 2f) -
                            textSize.Y / 2),
                        0xFFFFFFFF,
                        text
                    );
                }
                else
                {
                    markerStripHeight = timelineFramesMarkerHeight;
                }

                _painter.AddLine(
                    new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame,
                        drawOriginY + sequenceHeaderHeight - markerStripHeight),
                    new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame,
                        drawOriginY + sequenceHeaderHeight),
                    0xFFFFFFFF
                );

                if (_previewService.HasCached(i, ImagePresence.GPU))
                {
                    _painter.AddRectFilled(
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame + 1,
                            drawOriginY + sequenceHeaderHeight - markerStripHeight),
                        new Vector2(drawOriginX + trackHeaderWidth + (i + 1) * widthPerFrame - 1,
                            drawOriginY + sequenceHeaderHeight),
                        0xFF00FF00
                    );
                }
                else if (_previewService.HasCached(i, ImagePresence.DISK))
                {
                    _painter.AddRectFilled(
                        new Vector2(drawOriginX + trackHeaderWidth + i * widthPerFrame + 1,
                            drawOriginY + sequenceHeaderHeight - markerStripHeight),
                        new Vector2(drawOriginX + trackHeaderWidth + (i + 1) * widthPerFrame - 1,
                            drawOriginY + sequenceHeaderHeight),
                        0xFFFF0000
                    );
                }
            }
        }

        public void DrawCursor(
            float drawOriginX,
            float drawOriginY,
            float trackHeaderWidth,
            int sequenceCursorTimeFrame,
            ref int cursorDragFrames,
            float widthPerFrame,
            int sequenceFrameLength,
            Vector2 cursorSize,
            Vector2 windowPosition,
            Vector2[] cursorShapePoints,
            Vector2[] cursorCurrentPoints,
            ref bool isDraggingCursor
        )
        {
            var cursorPosition =
                new Vector2(
                    drawOriginX + trackHeaderWidth + (sequenceCursorTimeFrame + cursorDragFrames) * widthPerFrame -
                    cursorSize.X / 2, drawOriginY);

            ImGui.SetCursorPos(cursorPosition - windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##cursor", cursorSize);

            if (ImGui.IsItemActive())
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();
                cursorDragFrames = (int) (mouseDragDelta.X / widthPerFrame);
                cursorDragFrames = Math.Clamp(cursorDragFrames, -sequenceCursorTimeFrame,
                    sequenceFrameLength - sequenceCursorTimeFrame);
                isDraggingCursor = true;
            }

            for (var i = 0; i < cursorShapePoints.Length; i++)
            {
                cursorCurrentPoints[i].X = cursorShapePoints[i].X + cursorPosition.X;
                cursorCurrentPoints[i].Y = cursorShapePoints[i].Y + cursorPosition.Y;
            }

            _painter.AddConvexPolyFilled(ref cursorCurrentPoints[0], 5, 0xFFAA6666);
        }

        public void ProcessGhostClip(
            ClipImGuiRenderer clipRenderer,
            GhostClip ghostClip,
            float drawOriginX,
            float drawOriginY,
            float trackHeaderWidth,
            float trackContentWidth,
            float sequenceFrameLength,
            float sequenceHeaderHeight,
            int trackMargin,
            int trackHeight
        )
        {
            var clip = ghostClip;
            var clipTopLeft = new Vector2(
                drawOriginX + trackHeaderWidth +
                trackContentWidth * ((float) (clip.StartFrame) / sequenceFrameLength),
                drawOriginY + sequenceHeaderHeight + (ghostClip.CurrentTrackIndex) * (trackHeight + trackMargin)
            );
            var clipBottomRight = new Vector2(
                drawOriginX + trackHeaderWidth + trackContentWidth *
                ((float) (clip.StartFrame + clip.FrameLength) / sequenceFrameLength),
                drawOriginY + sequenceHeaderHeight + (ghostClip.CurrentTrackIndex + 1) * trackHeight +
                (ghostClip.CurrentTrackIndex) * trackMargin
            );

            clipRenderer.RenderGhost(ref _painter, clip, ref clipTopLeft, ref clipBottomRight);
        }

        public void DrawTrackHead(
            int index,
            float drawOriginX,
            float drawOriginY,
            float sequenceHeaderHeight,
            int trackHeight,
            int trackMargin,
            float trackHeaderWidth
        )
        {
            _painter.AddRectFilled(
                new Vector2(drawOriginX, drawOriginY + sequenceHeaderHeight + index * (trackHeight + trackMargin)),
                new Vector2(drawOriginX + trackHeaderWidth,
                    drawOriginY + sequenceHeaderHeight + (index + 1) * trackHeight + index * trackMargin),
                0xFF444444
            );

            _painter.AddText(
                new Vector2(drawOriginX + TrackMarginLeft,
                    drawOriginY + sequenceHeaderHeight + index * (trackHeight + trackMargin)),
                0xFFFFFFFF, $"TRACK {index}");
        }

        public void DrawTrackContentBackground(int index,
            float drawOriginX,
            float drawOriginY,
            float sequenceHeaderHeight,
            int trackHeight,
            int trackMargin,
            float trackHeaderWidth,
            Vector2 contentRegionAvail)
        {
            _painter.AddRectFilled(
                new Vector2(drawOriginX + trackHeaderWidth,
                    drawOriginY + sequenceHeaderHeight + index * (trackHeight + trackMargin)),
                new Vector2(drawOriginX + contentRegionAvail.X,
                    drawOriginY + sequenceHeaderHeight + (index + 1) * trackHeight + index * trackMargin),
                0xFF222222
            );
        }

        public void DrawClip(
            EmptyClip clip,
            int index,
            ClipImGuiRenderer clipRenderer,
            EditingService editingService,
            float drawOriginX,
            float drawOriginY,
            float trackHeaderWidth,
            float trackContentWidth,
            float sequenceFrameLength,
            float sequenceHeaderHeight,
            int trackHeight,
            int trackMargin,
            Vector2 windowPosition
        )
        {
            var clipTopLeft = new Vector2(
                drawOriginX + trackHeaderWidth +
                trackContentWidth * ((float) (clip.StartFrame) / sequenceFrameLength),
                drawOriginY + sequenceHeaderHeight + (index) * (trackHeight + trackMargin)
            );
            var clipBottomRight = new Vector2(
                drawOriginX + trackHeaderWidth + trackContentWidth *
                ((float) (clip.StartFrame + clip.FrameLength) / sequenceFrameLength),
                drawOriginY + sequenceHeaderHeight + (index + 1) * trackHeight + (index) * trackMargin
            );

            clipRenderer.Render(ref _painter, clip, ref clipTopLeft, ref clipBottomRight);

            ImGui.SetCursorPos(clipTopLeft - windowPosition);
            ImGui.SetItemAllowOverlap();
            var temp = new Vector2(clipBottomRight.X - clipTopLeft.X, clipBottomRight.Y - clipTopLeft.Y);
            if (ImGui.InvisibleButton($"##clip{clip.Guid:N}",
                    temp))
            {
                editingService.SetSelectedClip(clip);
            }
        }

        public void RefreshPainter()
        {
            _painter = ImGui.GetWindowDrawList();
        }
    }
}