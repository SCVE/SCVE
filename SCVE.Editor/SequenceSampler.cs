using System.Numerics;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Imaging;
using SCVE.Editor.MemoryUtils;
using SCVE.Editor.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor
{
    public class SequenceSampler
    {
        private ByteArrayPool _pool;
        FontCollection fontCollection = new FontCollection();
        private Font font;

        private ClipEvaluator _clipEvaluator;

        public SequenceSampler(ClipEvaluator clipEvaluator)
        {
            _clipEvaluator = clipEvaluator;
            fontCollection.Add("assets/Font/arial.ttf");
            font = fontCollection.Get("arial").CreateFont(72);
        }

        public ThreeWayImage Sample(Sequence sequence, ScveVector2I renderResolution, int timeFrame)
        {
            _pool ??= new ByteArrayPool(renderResolution.X * renderResolution.Y * 4, 2);

            var previewPoolItem = _pool.GetFree();
            var clipPoolItem = _pool.GetFree();
            var previewImage = new CpuImage(previewPoolItem.Bytes, renderResolution.X, renderResolution.Y);
            var clipImage = new CpuImage(clipPoolItem.Bytes, renderResolution.X, renderResolution.Y);

            using var previewImageSharpImage =
                Image.WrapMemory<Rgba32>(previewImage.ToByteArray(), previewImage.Width, previewImage.Height);
            using var clipImageSharpImage =
                Image.WrapMemory<Rgba32>(clipImage.ToByteArray(), previewImage.Width, previewImage.Height);

            previewImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

            for (var i = sequence.Tracks.Count - 1; i >= 0; i--)
            {
                var track = sequence.Tracks[i];

                for (var j = 0; j < track.AssetClips.Count; j++)
                {
                    var clip = track.AssetClips[j];
                    if (clip.StartFrame > timeFrame || clip.EndFrame <= timeFrame)
                    {
                        continue;
                    }

                    if (_clipEvaluator.Evaluate(clip, timeFrame - clip.StartFrame, clipImage.ToByteArray(), renderResolution.X, renderResolution.Y))
                    {
                        previewImageSharpImage.Mutate(i => i.DrawImage(clipImageSharpImage, 1f));

                        clipImageSharpImage.Mutate(i => i.Clear(Color.Transparent));
                    }
                }
            }

            previewImageSharpImage.Mutate(i =>
                i.DrawText($"DEBUG FRAME RENDER: {timeFrame}", font, Color.Red, new PointF(10, 0)));

            _pool.Return(previewPoolItem);
            _pool.Return(clipPoolItem);
            
            return new ThreeWayImage(previewImage, timeFrame.ToString());
        }
    }
}