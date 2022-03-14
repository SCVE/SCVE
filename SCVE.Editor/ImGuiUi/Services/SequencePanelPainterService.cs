using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Services
{
    public class SequencePanelPainterService : IService
    {
        #region Constants

        // private const int TrackHeight = 20;
        // private const int TrackMargin = 5;
        //
        // private const int SequenceHeaderHeight = 20;
        //
        // private const int TimelineFrameMarkerHeight = 3;
        // private const int TimelineSecondsMarkerHeight = 8;
        //
        // private static readonly Vector2 CursorSize = new(10, 20);
        //
        // private readonly Vector2[] _cursorShapePoints;
        // private readonly Vector2[] _cursorCurrentPoints;

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

        private readonly SettingsService _settingsService;

        public SequencePanelPainterService(SettingsService settingsService)
        {
            _settingsService = settingsService;
            _clipRenderer = new ClipImGuiRenderer();
            // _cursorShapePoints = new[]
            // {
            //     // top-left
            //     new Vector2(),
            //     // top-right
            //     new(_settingsService.SettingsInstance.CursorSize.X, 0),
            //     // right mid
            //     new(_settingsService.SettingsInstance.CursorSize.X,
            //         _settingsService.SettingsInstance.CursorSize.Y / 2f),
            //     // bottom mid
            //     new(_settingsService.SettingsInstance.CursorSize.X / 2, _settingsService.SettingsInstance.CursorSize.Y),
            //     // left mid
            //     new(0, _settingsService.SettingsInstance.CursorSize.Y / 2f),
            // };
            // _cursorCurrentPoints = new Vector2[_cursorShapePoints.Length];
        }

        public void SetRenderData(int cursorDragFrames, Sequence sequence)
        {
            _painter = ImGui.GetWindowDrawList();
            _windowPosition = ImGui.GetWindowPos();
            _contentRegionMin = ImGui.GetWindowContentRegionMin();
            _contentRegionAvail = ImGui.GetContentRegionAvail();
            _drawOrigin = _windowPosition + _contentRegionMin;
            _windowContentWidth = _contentRegionAvail.X;
            _trackContentWidth = _windowContentWidth - _settingsService.SettingsInstance.TrackHeaderWidth;
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
                new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth, _drawOrigin.Y),
                new Vector2(_drawOrigin.X + _windowContentWidth,
                    _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight),
                0xFF333333
            );

            newCursorTimeFrame = _sequenceCursorTimeFrame;

            ImGui.SetCursorPos(new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth,
                _drawOrigin.Y) - _windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##timeline-header",
                new Vector2(_windowContentWidth - _settingsService.SettingsInstance.TrackHeaderWidth,
                    _settingsService.SettingsInstance.SequenceHeaderHeight));

            if (ImGui.IsItemActive())
            {
                int timelineClickedFrame =
                    (int) ((ImGui.GetMousePos().X - _drawOrigin.X -
                            _settingsService.SettingsInstance.TrackHeaderWidth) / _widthPerFrame);
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
                    markerStripHeight = _settingsService.SettingsInstance.TimelineSecondsMarkerHeight;
                    var secondsText = $"{i / _sequenceFPS}";
                    var secondsTextSize = ImGui.CalcTextSize(secondsText);

                    // seconds text markers
                    _painter.AddText(
                        new Vector2(
                            _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + i * _widthPerFrame -
                            secondsTextSize.X / 2,
                            _drawOrigin.Y + ((_settingsService.SettingsInstance.SequenceHeaderHeight -
                                              _settingsService.SettingsInstance.TimelineSecondsMarkerHeight) / 2f) -
                            secondsTextSize.Y / 2),
                        0xFFFFFFFF,
                        secondsText
                    );
                }
                else
                {
                    markerStripHeight = _settingsService.SettingsInstance.TimelineFrameMarkerHeight;
                }

                _painter.AddLine(
                    new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + i * _widthPerFrame,
                        _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight - markerStripHeight),
                    new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + i * _widthPerFrame,
                        _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight),
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
            var pMin = new Vector2(
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + index * _widthPerFrame + 1,
                _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight -
                _settingsService.SettingsInstance.TimelineFrameMarkerHeight);
            var pMax = new Vector2(
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + (index + 1) * _widthPerFrame - 1,
                _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight);
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
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth +
                (_sequenceCursorTimeFrame + _cursorDragFrames) * _widthPerFrame -
                _settingsService.SettingsInstance.CursorSize.X / 2,
                _drawOrigin.Y
            );

            newCursorDragFrames = _cursorDragFrames;

            ImGui.SetCursorPos(cursorPosition - _windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##cursor", _settingsService.SettingsInstance.CursorSize);

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

            for (var i = 0; i < _settingsService.SettingsInstance.CursorShapePoints.Length; i++)
            {
                _settingsService.SettingsInstance.CursorCurrentPoints[i].X = _settingsService.SettingsInstance.CursorShapePoints[i].X + cursorPosition.X;
                _settingsService.SettingsInstance.CursorCurrentPoints[i].Y = _settingsService.SettingsInstance.CursorShapePoints[i].Y + cursorPosition.Y;
            }

            _painter.AddConvexPolyFilled(ref _settingsService.SettingsInstance.CursorCurrentPoints[0], 5, 0xFFAA6666);

            return isDragging;
        }

        public void DrawGhostClip(GhostClip ghostClip)
        {
            var clipTopLeft = new Vector2(
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth +
                _trackContentWidth * ((float) (ghostClip.CurrentStartFrame) / _sequenceFrameLength),
                _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight + (ghostClip.CurrentTrackIndex) *
                (_settingsService.SettingsInstance.TrackHeight + _settingsService.SettingsInstance.TrackMargin)
            );
            var clipBottomRight = new Vector2(
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + _trackContentWidth *
                ((float) (ghostClip.CurrentStartFrame + ghostClip.CurrentFrameLength) / _sequenceFrameLength),
                _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight +
                (ghostClip.CurrentTrackIndex + 1) * _settingsService.SettingsInstance.TrackHeight +
                (ghostClip.CurrentTrackIndex) * _settingsService.SettingsInstance.TrackMargin
            );

            _clipRenderer.RenderGhost(ref _painter, ghostClip, ref clipTopLeft, ref clipBottomRight);
        }

        public void DrawTrackHead(int index)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X,
                    _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight + index *
                    (_settingsService.SettingsInstance.TrackHeight + _settingsService.SettingsInstance.TrackMargin)),
                new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth,
                    _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight +
                    (index + 1) * _settingsService.SettingsInstance.TrackHeight +
                    index * _settingsService.SettingsInstance.TrackMargin),
                0xFF444444
            );

            _painter.AddText(
                new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackMarginLeft,
                    _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight + index *
                    (_settingsService.SettingsInstance.TrackHeight + _settingsService.SettingsInstance.TrackMargin)),
                0xFFFFFFFF, $"TRACK {index}");
        }

        public void DrawTrackContentBackground(int index)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth,
                    _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight + index *
                    (_settingsService.SettingsInstance.TrackHeight + _settingsService.SettingsInstance.TrackMargin)),
                new Vector2(_drawOrigin.X + _contentRegionAvail.X,
                    _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight +
                    (index + 1) * _settingsService.SettingsInstance.TrackHeight +
                    index * _settingsService.SettingsInstance.TrackMargin),
                0xFF222222
            );
        }

        public bool DrawClip(EmptyClip clip, int trackIndex, out bool isActive, out int deltaFrames,
            out int deltaTracks)
        {
            var clipTopLeft = new Vector2(
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth +
                _trackContentWidth * ((float) (clip.StartFrame) / _sequenceFrameLength),
                _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight + (trackIndex) *
                (_settingsService.SettingsInstance.TrackHeight + _settingsService.SettingsInstance.TrackMargin)
            );
            var clipBottomRight = new Vector2(
                _drawOrigin.X + _settingsService.SettingsInstance.TrackHeaderWidth + _trackContentWidth *
                ((float) (clip.StartFrame + clip.FrameLength) / _sequenceFrameLength),
                _drawOrigin.Y + _settingsService.SettingsInstance.SequenceHeaderHeight +
                (trackIndex + 1) * _settingsService.SettingsInstance.TrackHeight +
                (trackIndex) * _settingsService.SettingsInstance.TrackMargin
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
                deltaTracks = (int) (mouseDragDelta.Y / (_settingsService.SettingsInstance.TrackHeight +
                                                         _settingsService.SettingsInstance.TrackMargin));
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