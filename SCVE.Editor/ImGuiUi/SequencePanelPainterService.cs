using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanelPainterService : IService
    {
        #region Constants

        private const int TrackMarginLeft = 10;
        private const int TrackHeaderWidth = 70;

        private const int TrackHeight = 20;
        private const int TrackMargin = 5;

        private const int SequenceHeaderHeight = 20;

        private const int TimelineFrameMarkerHeight = 3;
        private const int TimelineSecondsMarkerHeight = 8;

        private static readonly Vector2 CursorSize = new(10, 20);

        private static readonly Vector2[] CursorShapePoints =
        {
            // top-left
            new(),
            // top-right
            new(CursorSize.X, 0),
            // right mid
            new(CursorSize.X, CursorSize.Y / 2f),
            // bottom mid
            new(CursorSize.X / 2, CursorSize.Y),
            // left mid
            new(0, CursorSize.Y / 2f),
        };

        private static readonly Vector2[] CursorCurrentPoints = new Vector2[CursorShapePoints.Length];

        #endregion

        #region Privates

        private ImDrawListPtr _painter;
        private Vector2 _windowPosition;
        private Vector2 _contentRegionMin;
        private Vector2 _contentRegionAvail;
        private Vector2 _drawOrigin;
        private float _windowContentWidth;
        private float _trackContentWidth;
        private float _widthPerFrame;
        private int _sequenceFPS;
        private int _sequenceCursorTimeFrame;
        private int _sequenceFrameLength;
        private int _tracksCount;

        private readonly ClipImGuiRenderer _clipRenderer;
        private int _cursorDragFrames;

        #endregion

        public SequencePanelPainterService()
        {
            _clipRenderer = new ClipImGuiRenderer();
        }

        public void SetRenderData(int cursorDragFrames, Sequence sequence)
        {
            _painter = ImGui.GetWindowDrawList();
            _windowPosition = ImGui.GetWindowPos();
            _contentRegionMin = ImGui.GetWindowContentRegionMin();
            _contentRegionAvail = ImGui.GetContentRegionAvail();
            _drawOrigin = _windowPosition + _contentRegionMin;
            _windowContentWidth = _contentRegionAvail.X;
            _trackContentWidth = _windowContentWidth - TrackHeaderWidth;
            _cursorDragFrames = cursorDragFrames;

            _widthPerFrame = _trackContentWidth / sequence.FrameLength;
            _tracksCount = sequence.Tracks.Count;
            _sequenceFPS = sequence.FPS;
            _sequenceCursorTimeFrame = sequence.CursorTimeFrame;
            _sequenceFrameLength = sequence.FrameLength;
        }

        public bool DrawSequenceHeader(out int newCursorTimeFrame)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X + TrackHeaderWidth, _drawOrigin.Y),
                new Vector2(_drawOrigin.X + _windowContentWidth, _drawOrigin.Y + SequenceHeaderHeight),
                0xFF333333
            );

            newCursorTimeFrame = _sequenceCursorTimeFrame;

            ImGui.SetCursorPos(new Vector2(_drawOrigin.X + TrackHeaderWidth, _drawOrigin.Y) - _windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##timeline-header",
                new Vector2(_windowContentWidth - TrackHeaderWidth, SequenceHeaderHeight));

            if (ImGui.IsItemActive())
            {
                int timelineClickedFrame =
                    (int) ((ImGui.GetMousePos().X - _drawOrigin.X - TrackHeaderWidth) / _widthPerFrame);
                if (_sequenceCursorTimeFrame != timelineClickedFrame)
                {
                    newCursorTimeFrame = Math.Clamp(timelineClickedFrame, 0, _sequenceFrameLength);
                    return true;
                }
            }

            return false;
        }

        public void DrawSequenceFrameMarkers()
        {
            for (int i = 0; i < _sequenceFrameLength; i++)
            {
                int markerStripHeight;
                if (i % _sequenceFPS == 0)
                {
                    markerStripHeight = TimelineSecondsMarkerHeight;
                    var secondsText = $"{i / _sequenceFPS}";
                    var secondsTextSize = ImGui.CalcTextSize(secondsText);

                    // seconds text markers
                    _painter.AddText(
                        new Vector2(_drawOrigin.X + TrackHeaderWidth + i * _widthPerFrame - secondsTextSize.X / 2,
                            _drawOrigin.Y + ((SequenceHeaderHeight - TimelineSecondsMarkerHeight) / 2f) -
                            secondsTextSize.Y / 2),
                        0xFFFFFFFF,
                        secondsText
                    );
                }
                else
                {
                    markerStripHeight = TimelineFrameMarkerHeight;
                }

                _painter.AddLine(
                    new Vector2(_drawOrigin.X + TrackHeaderWidth + i * _widthPerFrame,
                        _drawOrigin.Y + SequenceHeaderHeight - markerStripHeight),
                    new Vector2(_drawOrigin.X + TrackHeaderWidth + i * _widthPerFrame,
                        _drawOrigin.Y + SequenceHeaderHeight),
                    0xFFFFFFFF
                );

                // if (_previewService.HasCached(i, ImagePresence.GPU))
                // {
                //     DrawFrameMarker(i, 0xFF00FF00);
                // }
                // else if (_previewService.HasCached(i, ImagePresence.DISK))
                // {
                //     DrawFrameMarker(i, 0xFFFF0000);
                // }
            }
        }

        private void DrawFrameMarker(int index, uint color)
        {
            var pMin = new Vector2(_drawOrigin.X + TrackHeaderWidth + index * _widthPerFrame + 1,
                _drawOrigin.Y + SequenceHeaderHeight - TimelineFrameMarkerHeight);
            var pMax = new Vector2(_drawOrigin.X + TrackHeaderWidth + (index + 1) * _widthPerFrame - 1,
                _drawOrigin.Y + SequenceHeaderHeight);
            _painter.AddRectFilled(
                pMin,
                pMax,
                color
            );
        }

        public bool DrawCursor(out int newCursorDragFrames)
        {
            bool isDragging = false;
            var cursorPosition = new Vector2(
                _drawOrigin.X + TrackHeaderWidth + (_sequenceCursorTimeFrame + _cursorDragFrames) * _widthPerFrame - CursorSize.X / 2,
                _drawOrigin.Y
            );

            newCursorDragFrames = _cursorDragFrames;

            ImGui.SetCursorPos(cursorPosition - _windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##cursor", CursorSize);

            if (ImGui.IsItemActive())
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();
                newCursorDragFrames = (int) (mouseDragDelta.X / _widthPerFrame);
                newCursorDragFrames = Math.Clamp(
                    newCursorDragFrames,
                    -_sequenceCursorTimeFrame,
                    _sequenceFrameLength - _sequenceCursorTimeFrame
                );
                isDragging = true;
            }

            for (var i = 0; i < CursorShapePoints.Length; i++)
            {
                CursorCurrentPoints[i].X = CursorShapePoints[i].X + cursorPosition.X;
                CursorCurrentPoints[i].Y = CursorShapePoints[i].Y + cursorPosition.Y;
            }

            _painter.AddConvexPolyFilled(ref CursorCurrentPoints[0], 5, 0xFFAA6666);

            return isDragging;
        }

        public void DrawGhostClip(GhostClip ghostClip)
        {
            var clipTopLeft = new Vector2(
                _drawOrigin.X + TrackHeaderWidth +
                _trackContentWidth * ((float) (ghostClip.CurrentStartFrame) / _sequenceFrameLength),
                _drawOrigin.Y + SequenceHeaderHeight + (ghostClip.CurrentTrackIndex) * (TrackHeight + TrackMargin)
            );
            var clipBottomRight = new Vector2(
                _drawOrigin.X + TrackHeaderWidth + _trackContentWidth *
                ((float) (ghostClip.CurrentStartFrame + ghostClip.CurrentFrameLength) / _sequenceFrameLength),
                _drawOrigin.Y + SequenceHeaderHeight + (ghostClip.CurrentTrackIndex + 1) * TrackHeight +
                (ghostClip.CurrentTrackIndex) * TrackMargin
            );

            _clipRenderer.RenderGhost(ref _painter, ghostClip, ref clipTopLeft, ref clipBottomRight);
        }

        public void DrawTrackHead(int index)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X, _drawOrigin.Y + SequenceHeaderHeight + index * (TrackHeight + TrackMargin)),
                new Vector2(_drawOrigin.X + TrackHeaderWidth,
                    _drawOrigin.Y + SequenceHeaderHeight + (index + 1) * TrackHeight + index * TrackMargin),
                0xFF444444
            );

            _painter.AddText(
                new Vector2(_drawOrigin.X + TrackMarginLeft,
                    _drawOrigin.Y + SequenceHeaderHeight + index * (TrackHeight + TrackMargin)),
                0xFFFFFFFF, $"TRACK {index}");
        }

        public void DrawTrackContentBackground(int index)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X + TrackHeaderWidth,
                    _drawOrigin.Y + SequenceHeaderHeight + index * (TrackHeight + TrackMargin)),
                new Vector2(_drawOrigin.X + _contentRegionAvail.X,
                    _drawOrigin.Y + SequenceHeaderHeight + (index + 1) * TrackHeight + index * TrackMargin),
                0xFF222222
            );
        }

        public bool DrawClip(EmptyClip clip, int trackIndex, out bool isActive, out int deltaFrames, out int deltaTracks)
        {
            var clipTopLeft = new Vector2(
                _drawOrigin.X + TrackHeaderWidth +
                _trackContentWidth * ((float) (clip.StartFrame) / _sequenceFrameLength),
                _drawOrigin.Y + SequenceHeaderHeight + (trackIndex) * (TrackHeight + TrackMargin)
            );
            var clipBottomRight = new Vector2(
                _drawOrigin.X + TrackHeaderWidth + _trackContentWidth *
                ((float) (clip.StartFrame + clip.FrameLength) / _sequenceFrameLength),
                _drawOrigin.Y + SequenceHeaderHeight + (trackIndex + 1) * TrackHeight + (trackIndex) * TrackMargin
            );

            _clipRenderer.Render(ref _painter, clip, ref clipTopLeft, ref clipBottomRight);

            ImGui.SetCursorPos(clipTopLeft - _windowPosition);
            // this messes up with click detection, making mouse a god-ray, punching through all clips
            // ImGui.SetItemAllowOverlap();

            var clipSize = clipBottomRight - clipTopLeft;

            var isClicked = ImGui.InvisibleButton($"##clip{clip.Guid:N}", clipSize);
            isActive = ImGui.IsItemActive();

            if (isActive)
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();

                deltaFrames = (int) (mouseDragDelta.X / _widthPerFrame);
                deltaTracks = (int) (mouseDragDelta.Y / (TrackHeight + TrackMargin));
            }
            else
            {
                deltaFrames = 0;
                deltaTracks = 0;
            }

            return isClicked;
        }
    }
}