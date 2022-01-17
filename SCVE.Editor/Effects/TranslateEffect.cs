using ImGuiNET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class TranslateEffect : IEffect
    {
        public int X { get; set; }

        public int Y { get; set; }

        public ImageFrame Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImageFrame = effectApplicationContext.ImageFrame;
            var dstImageFrame = new ImageFrame(srcImageFrame.Width, srcImageFrame.Height);
            dstImageFrame.CreateImageSharpWrapper();

            dstImageFrame.ImageSharpImage.Mutate(i => i.DrawImage(srcImageFrame.ImageSharpImage, new Point(X, Y), 1));

            return dstImageFrame;
        }

        public void OnImGuiRender()
        {
            int x = X;
            if (ImGui.SliderInt("X", ref x, -1000, 1000))
            {
                X = x;
            }

            int y = Y;
            if (ImGui.SliderInt("Y", ref y, -1000, 1000))
            {
                Y = y;
            }
        }
    }
}