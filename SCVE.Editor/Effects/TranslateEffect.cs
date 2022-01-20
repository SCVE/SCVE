using ImGuiNET;
using SCVE.Editor.Modules;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class TranslateEffect : IEffect
    {
        public int X { get; set; }

        public int Y { get; set; }

        private EditingModule _editingModule;
        private PreviewModule _previewModule;

        public TranslateEffect()
        {
            _editingModule = EditorApp.Modules.Get<EditingModule>();
            _previewModule = EditorApp.Modules.Get<PreviewModule>();
        }

        public ImageFrame Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImageFrame = effectApplicationContext.SourceImageFrame;

            var clone = srcImageFrame.ImageSharpImage.Clone();
            clone.Mutate(i => i.Resize((int)(srcImageFrame.Width * X), (int)(srcImageFrame.Height * Y)));

            srcImageFrame.ImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

            srcImageFrame.ImageSharpImage.Mutate(i => i.DrawImage(clone, new Point(X, Y), 1));

            return srcImageFrame;
        }

        public void OnImGuiRender()
        {
            int x = X;
            if (ImGui.SliderInt("X", ref x, -1000, 1000))
            {
                X = x;
                _previewModule.InvalidateSampledFrame(_editingModule.OpenedSequence.CursorTimeFrame);
            }

            int y = Y;
            if (ImGui.SliderInt("Y", ref y, -1000, 1000))
            {
                Y = y;
                _previewModule.InvalidateSampledFrame(_editingModule.OpenedSequence.CursorTimeFrame);
            }
        }
    }
}