using ImGuiNET;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class ScaleEffect : IEffect
    {
        public float X { get; set; } = 1;

        public float Y { get; set; } = 1;

        public ImageFrame Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImageFrame = effectApplicationContext.ImageFrame;
            var dstImageFrame = new ImageFrame(srcImageFrame.Width, srcImageFrame.Height);
            dstImageFrame.CreateImageSharpWrapper();

            var clone = srcImageFrame.ImageSharpImage.Clone();
            clone.Mutate(i => i.Resize((int)(srcImageFrame.Width * X), (int)(srcImageFrame.Height * Y)));

            dstImageFrame.ImageSharpImage.Mutate(i => i.DrawImage(clone, 1));

            return dstImageFrame;
        }

        public void OnImGuiRender()
        {
            float x = X;
            if (ImGui.SliderFloat("X", ref x, 0, 5))
            {
                X = x;
            }

            float y = Y;
            if (ImGui.SliderFloat("Y", ref y, 0, 5))
            {
                Y = y;
            }
        }
    }
}