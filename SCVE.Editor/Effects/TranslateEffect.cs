using ImGuiNET;
using SCVE.Editor.Editing;
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
        private Clip _clip;

        public TranslateEffect()
        {
            _editingModule = EditorApp.Modules.Get<EditingModule>();
            _previewModule = EditorApp.Modules.Get<PreviewModule>();
        }

        public void AttachToClip(Clip clip)
        {
            _clip = clip;
        }

        public void DeAttachFromClip()
        {
            _clip = null;
        }

        public ImageFrame Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImageFrame = effectApplicationContext.SourceImageFrame;

            var clone = srcImageFrame.ImageSharpImage.Clone();

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
                _previewModule.InvalidateRange(_clip.StartFrame, _clip.FrameLength);
            }

            int y = Y;
            if (ImGui.SliderInt("Y", ref y, -1000, 1000))
            {
                Y = y;
                _previewModule.InvalidateRange(_clip.StartFrame, _clip.FrameLength);
            }
        }
    }
}