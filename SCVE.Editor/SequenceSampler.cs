using SCVE.Editor.Editing.Editing;
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

        public ThreeWayImage Sample(Sequence sequence, int timeFrame)
        {
            if (_pool is null)
            {
                _pool = new ByteArrayPool((int) sequence.Resolution.X * (int) sequence.Resolution.Y * 4, 2);
            }

            var previewPoolItem = _pool.GetFree();
            var clipPoolItem = _pool.GetFree();
            var previewImage = new CpuImage(previewPoolItem.Bytes, (int) sequence.Resolution.X, (int) sequence.Resolution.Y);
            var clipImage = new CpuImage(clipPoolItem.Bytes, (int) sequence.Resolution.X, (int) sequence.Resolution.Y);

            using var previewImageSharpImage =
                Image.WrapMemory<Rgba32>(previewImage.ToByteArray(), previewImage.Width, previewImage.Height);
            using var clipImageSharpImage =
                Image.WrapMemory<Rgba32>(clipImage.ToByteArray(), previewImage.Width, previewImage.Height);

            previewImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

            for (var i = sequence.Tracks.Count - 1; i >= 0; i--)
            {
                var track = sequence.Tracks[i];
                if (track.StartFrame > timeFrame || track.EndFrame <= timeFrame)
                {
                    continue;
                }

                for (var j = 0; j < track.Clips.Count; j++)
                {
                    var clip = track.Clips[j];
                    if (clip.StartFrame > timeFrame || clip.EndFrame <= timeFrame)
                    {
                        continue;
                    }

                    if (_clipEvaluator.Evaluate(clip, timeFrame - clip.StartFrame, clipImage.ToByteArray(), (int) sequence.Resolution.X, (int) sequence.Resolution.Y))
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