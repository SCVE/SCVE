using System;
using System.ComponentModel;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.ImGuiUi.Models;
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
        private int _cursorFrame;
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
        }

        public void SetRenderData(int cursorDragFrames, int cursorFrame, Sequence sequence)
        {
            _painter = ImGui.GetWindowDrawList();
            _windowPosition = ImGui.GetWindowPos();
            _contentRegionMin = ImGui.GetWindowContentRegionMin();
            _contentRegionAvail = ImGui.GetContentRegionAvail();
            _drawOrigin = _windowPosition + _contentRegionMin;
            _windowContentWidth = _contentRegionAvail.X;
            _trackContentWidth = _windowContentWidth - Settings.Instance.TrackHeaderWidth;
            _cursorDragFrames = cursorDragFrames;
            _cursorFrame = cursorFrame;

            _widthPerFrame = _trackContentWidth / sequence.FrameLength;
            _tracksCount = sequence.Tracks.Count;
            _sequenceFPS = sequence.FPS;
            _sequenceFrameLength = sequence.FrameLength;
        }

        public bool DrawSequenceHeader(out int newCursorTimeFrame)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X + Settings.Instance.TrackHeaderWidth, _drawOrigin.Y),
                new Vector2(_drawOrigin.X + _windowContentWidth,
                    _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight),
                0xFF333333
            );

            newCursorTimeFrame = _cursorFrame;

            ImGui.SetCursorPos(new Vector2(_drawOrigin.X + Settings.Instance.TrackHeaderWidth,
                _drawOrigin.Y) - _windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##timeline-header",
                new Vector2(_windowContentWidth - Settings.Instance.TrackHeaderWidth,
                    Settings.Instance.SequenceHeaderHeight));

            if (ImGui.IsItemActive())
            {
                int timelineClickedFrame =
                    (int) ((ImGui.GetMousePos().X - _drawOrigin.X -
                            Settings.Instance.TrackHeaderWidth) / _widthPerFrame);
                if (_cursorFrame != timelineClickedFrame)
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
                    markerStripHeight = Settings.Instance.TimelineSecondsMarkerHeight;
                    var secondsText = $"{i / _sequenceFPS}";
                    var secondsTextSize = ImGui.CalcTextSize(secondsText);

                    // seconds text markers
                    _painter.AddText(
                        new Vector2(
                            _drawOrigin.X + Settings.Instance.TrackHeaderWidth + i * _widthPerFrame -
                            secondsTextSize.X / 2,
                            _drawOrigin.Y + ((Settings.Instance.SequenceHeaderHeight -
                                              Settings.Instance.TimelineSecondsMarkerHeight) / 2f) -
                            secondsTextSize.Y / 2),
                        0xFFFFFFFF,
                        secondsText
                    );
                }
                else
                {
                    markerStripHeight = Settings.Instance.TimelineFrameMarkerHeight;
                }

                _painter.AddLine(
                    new Vector2(_drawOrigin.X + Settings.Instance.TrackHeaderWidth + i * _widthPerFrame,
                        _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight - markerStripHeight),
                    new Vector2(_drawOrigin.X + Settings.Instance.TrackHeaderWidth + i * _widthPerFrame,
                        _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight),
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
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth + index * _widthPerFrame + 1,
                _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight -
                Settings.Instance.TimelineFrameMarkerHeight);
            var pMax = new Vector2(
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth + (index + 1) * _widthPerFrame - 1,
                _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight);
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
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth +
                (_cursorFrame + _cursorDragFrames) * _widthPerFrame -
                Settings.Instance.CursorSize.X / 2,
                _drawOrigin.Y
            );

            newCursorDragFrames = _cursorDragFrames;

            ImGui.SetCursorPos(cursorPosition - _windowPosition);
            ImGui.SetItemAllowOverlap();
            ImGui.InvisibleButton($"##cursor", Settings.Instance.CursorSize);

            if (ImGui.IsItemActive())
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();
                newCursorDragFrames = (int) (mouseDragDelta.X / _widthPerFrame);
                newCursorDragFrames = Math.Clamp(
                    newCursorDragFrames,
                    -_cursorFrame,
                    _sequenceFrameLength - _cursorFrame
                );
                isDragging = true;
            }

            for (var i = 0; i < Settings.Instance.CursorShapePoints.Length; i++)
            {
                // TODO: don't write to settings, WTF
                Settings.Instance.CursorCurrentPoints[i].X =
                    Settings.Instance.CursorShapePoints[i].X + cursorPosition.X;
                Settings.Instance.CursorCurrentPoints[i].Y =
                    Settings.Instance.CursorShapePoints[i].Y + cursorPosition.Y;
            }

            _painter.AddConvexPolyFilled(ref Settings.Instance.CursorCurrentPoints[0], 5, 0xFFAA6666);

            return isDragging;
        }

        public void DrawGhostClip(GhostClip ghostClip)
        {
            var clipTopLeft = new Vector2(
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth +
                _trackContentWidth * ((float) (ghostClip.CurrentStartFrame) / _sequenceFrameLength),
                _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight + (ghostClip.CurrentTrackIndex) *
                (Settings.Instance.TrackHeight + Settings.Instance.TrackMargin)
            );
            var clipBottomRight = new Vector2(
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth + _trackContentWidth *
                ((float) (ghostClip.CurrentStartFrame + ghostClip.CurrentFrameLength) / _sequenceFrameLength),
                _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight +
                (ghostClip.CurrentTrackIndex + 1) * Settings.Instance.TrackHeight +
                (ghostClip.CurrentTrackIndex) * Settings.Instance.TrackMargin
            );

            _clipRenderer.RenderGhost(ref _painter, ghostClip, ref clipTopLeft, ref clipBottomRight);
        }

        public void DrawTrackHead(int index)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X,
                    _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight + index *
                    (Settings.Instance.TrackHeight + Settings.Instance.TrackMargin)),
                new Vector2(_drawOrigin.X + Settings.Instance.TrackHeaderWidth,
                    _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight +
                    (index + 1) * Settings.Instance.TrackHeight +
                    index * Settings.Instance.TrackMargin),
                0xFF444444
            );

            _painter.AddText(
                new Vector2(_drawOrigin.X + Settings.Instance.TrackMarginLeft,
                    _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight + index *
                    (Settings.Instance.TrackHeight + Settings.Instance.TrackMargin)),
                0xFFFFFFFF, $"TRACK {index}");
        }

        public void DrawTrackContentBackground(int index)
        {
            _painter.AddRectFilled(
                new Vector2(_drawOrigin.X + Settings.Instance.TrackHeaderWidth,
                    _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight + index *
                    (Settings.Instance.TrackHeight + Settings.Instance.TrackMargin)),
                new Vector2(_drawOrigin.X + _contentRegionAvail.X,
                    _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight +
                    (index + 1) * Settings.Instance.TrackHeight +
                    index * Settings.Instance.TrackMargin),
                0xFF222222
            );
        }

        private void DrawClipHead(Clip clip, Vector2 position, Vector2 size, out bool isClicked, out bool isActive)
        {
            ImGui.SetCursorPos(position - _windowPosition);
            isClicked = ImGui.Button($"##clip-left{clip.Guid:N}", size);
            isActive = ImGui.IsItemActive();
        }

        private void DrawClipBody(Clip clip, Vector2 position, float marginLeft, Vector2 size, out bool isClicked, out bool isActive)
        {
            ImGui.SetCursorPos((position - _windowPosition) + new Vector2(marginLeft, 0));
            isClicked = ImGui.Button($"##clip-body{clip.Guid:N}", size);
            isActive = ImGui.IsItemActive();
        }

        private void DrawClipTail(Clip clip, Vector2 position, float marginLeft, Vector2 clipSize,
            out bool isClicked, out bool isActive)
        {
            ImGui.SetCursorPos((position - _windowPosition) + new Vector2(marginLeft, 0));
            isClicked = ImGui.Button($"##clip-right{clip.Guid:N}", clipSize);
            isActive = ImGui.IsItemActive();
        }

        public void DrawClip(Clip clip, int trackIndex, ref ClipManipulationData clipManipulationData)
        {
            var clipTopLeft = new Vector2(
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth +
                _trackContentWidth * ((float) (clip.StartFrame) / _sequenceFrameLength),
                _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight + (trackIndex) *
                (Settings.Instance.TrackHeight + Settings.Instance.TrackMargin)
            );
            var clipBottomRight = new Vector2(
                _drawOrigin.X + Settings.Instance.TrackHeaderWidth + _trackContentWidth *
                ((float) (clip.StartFrame + clip.FrameLength) / _sequenceFrameLength),
                _drawOrigin.Y + Settings.Instance.SequenceHeaderHeight +
                (trackIndex + 1) * Settings.Instance.TrackHeight +
                (trackIndex) * Settings.Instance.TrackMargin
            );

            _clipRenderer.Render(ref _painter, clip, clipTopLeft, clipBottomRight);

            // this messes up with click detection, making mouse a god-ray, punching through all clips
            // ImGui.SetItemAllowOverlap();

            var clipSize = clipBottomRight - clipTopLeft;
            
            CalcClipParts(clip.FrameLength, clipSize, out var clipLeftSize, out var clipBodySize, out var clipRightSize);

            DrawClipHead(clip, clipTopLeft, clipLeftSize, out clipManipulationData.IsHeadClicked, out clipManipulationData.IsHeadActive);

            DrawClipBody(clip, clipTopLeft, clipLeftSize.X, clipBodySize, out clipManipulationData.IsBodyClicked, out clipManipulationData.IsBodyActive);

            DrawClipTail(clip, clipTopLeft, clipLeftSize.X + clipBodySize.X, clipRightSize,
                out clipManipulationData.IsTailClicked, out clipManipulationData.IsTailActive);

            if (clipManipulationData.IsBodyActive)
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();

                clipManipulationData.BodyDragDeltaFrames = (int) (mouseDragDelta.X / _widthPerFrame);
                clipManipulationData.DeltaTracks = (int) (mouseDragDelta.Y / (Settings.Instance.TrackHeight +
                                                                            Settings.Instance.TrackMargin));
            }
            else
            {
                clipManipulationData.BodyDragDeltaFrames = 0;
                clipManipulationData.DeltaTracks = 0;
            }

            if (clipManipulationData.IsHeadActive)
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();

                clipManipulationData.HeadDragDeltaFrames = (int) (mouseDragDelta.X / _widthPerFrame);
            }
            else
            {
                clipManipulationData.HeadDragDeltaFrames = 0;
            }

            if (clipManipulationData.IsTailActive)
            {
                var mouseDragDelta = ImGui.GetMouseDragDelta();

                clipManipulationData.TailDragDeltaFrames = (int) (mouseDragDelta.X / _widthPerFrame);
            }
            else
            {
                clipManipulationData.TailDragDeltaFrames = 0;
            }
        }

        private void CalcClipParts(int clipFrameLength, Vector2 clipSize, out Vector2 clipLeftSize, out Vector2 clipBodySize,
            out Vector2 clipRightSize)
        {
            if (clipFrameLength > 4)
            {
                clipLeftSize = new Vector2(_widthPerFrame * 2, clipSize.Y);
                clipBodySize = new Vector2(clipSize.X - _widthPerFrame * 4, clipSize.Y);
                clipRightSize = new Vector2(_widthPerFrame * 2, clipSize.Y);
            }
            else if (clipFrameLength > 2)
            {
                clipLeftSize = new Vector2(_widthPerFrame, clipSize.Y);
                clipBodySize = new Vector2(clipSize.X - _widthPerFrame * 2, clipSize.Y);
                clipRightSize = new Vector2(_widthPerFrame, clipSize.Y);
            }
            else if (clipFrameLength > 1)
            {
                clipLeftSize = new Vector2(_widthPerFrame / 2, clipSize.Y);
                clipBodySize = new Vector2(clipSize.X - _widthPerFrame, clipSize.Y);
                clipRightSize = new Vector2(_widthPerFrame / 2, clipSize.Y);
            }
            else
            {
                clipLeftSize = new Vector2(_widthPerFrame / 4, clipSize.Y);
                clipBodySize = new Vector2(clipSize.X - _widthPerFrame / 2, clipSize.Y);
                clipRightSize = new Vector2(_widthPerFrame / 4, clipSize.Y);
            }
        }
    }
}