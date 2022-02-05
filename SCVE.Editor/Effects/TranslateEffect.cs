using ImGuiNET;
using SCVE.Editor.Editing;
using SCVE.Editor.Modules;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IImage = SCVE.Editor.Imaging.IImage;

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

        public IImage Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImage = effectApplicationContext.SourceImageFrame;

            using var srcImageSharpImage = Image.WrapMemory<Rgba32>(srcImage.ToByteArray(), srcImage.Width, srcImage.Height);

            var clone = srcImageSharpImage.Clone();

            srcImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

            srcImageSharpImage.Mutate(i => i.DrawImage(clone, new Point(X, Y), 1));

            return srcImage;
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