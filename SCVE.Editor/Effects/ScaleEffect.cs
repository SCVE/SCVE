using ImGuiNET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class ScaleEffect : IEffect
    {
        public float X { get; set; } = 1;

        public float Y { get; set; } = 1;

        public ImageFrame Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImageFrame = effectApplicationContext.SourceImageFrame;
            var dstSizeX = (int)(srcImageFrame.Width * X);
            var dstSizeY = (int)(srcImageFrame.Height * Y);

            using (var clone = srcImageFrame.ImageSharpImage.Clone(i => i.Resize(dstSizeX, dstSizeY)))
            {
                srcImageFrame.ImageSharpImage.Mutate(i => i.Clear(Color.Transparent).DrawImage(clone, 1));
            }

            return srcImageFrame;
        }

        public void OnImGuiRender()
        {
            float x = X;
            if (ImGui.SliderFloat("X", ref x, 0, 5))
            {
                X = x;
                EditorApp.Instance.MarkPreviewDirty();
            }

            float y = Y;
            if (ImGui.SliderFloat("Y", ref y, 0, 5))
            {
                Y = y;
                EditorApp.Instance.MarkPreviewDirty();
            }
        }
    }
}