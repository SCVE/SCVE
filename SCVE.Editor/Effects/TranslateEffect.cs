using ImGuiNET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class TranslateEffect : IEffect
    {
        public int X { get; set; }

        public int Y { get; set; }

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
                EditorApp.Instance.MarkPreviewDirty();
            }

            int y = Y;
            if (ImGui.SliderInt("Y", ref y, -1000, 1000))
            {
                Y = y;
                EditorApp.Instance.MarkPreviewDirty();
            }
        }
    }
}