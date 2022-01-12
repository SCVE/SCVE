using System;
using System.Linq;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing;
using Silk.NET.Maths;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanel : IImGuiRenderable
    {
        private float _currentScale = 1f;
        private static float HeaderHeight = 24f;

        private Sequence _sequence;

        private ClipImGuiRenderer _clipRenderer;

        public SequencePanel(Sequence sequence)
        {
            _sequence     = sequence;
            _clipRenderer = new ClipImGuiRenderer();
        }

        private Random _random = new Random();


        public void OnImGuiRender()
        {
            if (ImGui.Begin("Sequence Panel"))
            {
                var windowPos = ImGui.GetWindowPos();

                var painter = ImGui.GetWindowDrawList();

                var contentRegionMin = ImGui.GetWindowContentRegionMin();

                var contentRegionAvail = ImGui.GetContentRegionAvail();

                var fullWidth = contentRegionAvail.X;

                var trackHeight = 20;

                var trackMargin = 5;

                var drawOriginX = windowPos.X + contentRegionMin.X;
                var drawOriginY = windowPos.Y + contentRegionMin.Y;

                var mousePos = ImGui.GetMousePos();

                ImGui.PushClipRect(new Vector2(0, 0), new Vector2(200, 5000), false);

                var sequenceLength = _sequence.FrameLength;
                for (var i = 0; i < _sequence.Tracks.Count; i++)
                {
                    painter.AddRectFilled(
                        new Vector2(drawOriginX, drawOriginY + i * (trackHeight + trackMargin)),
                        new Vector2(drawOriginX + contentRegionAvail.X, drawOriginY + (i + 1) * trackHeight + i * trackMargin),
                        0xFF222222
                    );

                    for (int j = 0; j < _sequence.Tracks[i].Clips.Count; j++)
                    {
                        var clip = _sequence.Tracks[i].Clips[j];

                        var clipTopLeft = new Vector2(
                            drawOriginX + fullWidth * ((float)(clip.StartFrame) / sequenceLength),
                            drawOriginY + i * (trackHeight + trackMargin)
                        );
                        var clipBottomRight = new Vector2(
                            drawOriginX + fullWidth * ((float)(clip.StartFrame + clip.FrameLength) / sequenceLength),
                            drawOriginY + (i + 1) * trackHeight + i * trackMargin
                        );
                        _clipRenderer.Render(ref painter, clip, ref clipTopLeft, ref clipBottomRight);
                    }
                }

                ImGui.End();
            }
        }
    }
}