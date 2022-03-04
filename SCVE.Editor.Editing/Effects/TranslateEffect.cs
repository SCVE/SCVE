using ImGuiNET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Editing.Effects
{
    public class TranslateEffect : EffectBase
    {
        public event Action Updated;
        public int X { get; set; }

        public int Y { get; set; }

        public TranslateEffect()
        {
        }

        protected override void Algorithm(byte[] pixels, int width, int height)
        {
            using var srcImageSharpImage = Image.WrapMemory<Rgba32>(pixels, width, height);

            var clone = srcImageSharpImage.Clone();

            srcImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

            srcImageSharpImage.Mutate(i => i.DrawImage(clone, new Point(X, Y), 1));
        }
        
        protected override void OnImGuiRenderAlgorithm()
        {
            int x = X;
            if (ImGui.SliderInt("X", ref x, -1000, 1000))
            {
                X = x;
                Updated?.Invoke();
            }

            int y = Y;
            if (ImGui.SliderInt("Y", ref y, -1000, 1000))
            {
                Y = y;
                Updated?.Invoke();
            }
        }
    }
}