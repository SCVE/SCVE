using SCVE.Editor.Editing;
using SCVE.Editor.Effects;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using ImageFrame = SCVE.Editor.Effects.ImageFrame;

namespace SCVE.Editor
{
    public class SequenceSampler
    {
        FontCollection fontCollection = new FontCollection();
        private Font font;

        private ClipEvaluator _clipEvaluator = new ClipEvaluator();

        public SequenceSampler()
        {
            fontCollection.Install("assets/Font/arial.ttf");
            font = fontCollection.CreateFont("arial", 72);
        }

        public ImageFrame Sample(Sequence sequence, int timeFrame)
        {
            var previewImage = new ImageFrame((int)sequence.Resolution.X, (int)sequence.Resolution.Y);
            var clipImage   = new ImageFrame((int)sequence.Resolution.X, (int)sequence.Resolution.Y);
            previewImage.CreateImageSharpWrapper();
            clipImage.CreateImageSharpWrapper();

            previewImage.ImageSharpImage.Mutate(i => i.Clear(Color.Transparent));

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

                    if (_clipEvaluator.Evaluate(sequence, clip, timeFrame - clip.StartFrame, clipImage))
                    {
                        previewImage.ImageSharpImage.Mutate(i => i.DrawImage(clipImage.ImageSharpImage, 1f));

                        clipImage.ImageSharpImage.Mutate(i => i.Clear(Color.Transparent));
                    }
                }
            }

            previewImage.ImageSharpImage.Mutate(i => i.DrawText($"DEBUG FRAME RENDER: {timeFrame}", font, Color.Red, new PointF(0, 0)));

            return previewImage;
        }
    }
}