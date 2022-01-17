using System;
using System.IO;
using SCVE.Editor.Editing;
using SCVE.Editor.Effects;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor
{
    public class SequenceSampler
    {
        public Image<Rgba32> PreviewImage;

        FontCollection fontCollection = new FontCollection();
        private Font font;

        private ClipEvaluator _clipEvaluator = new ClipEvaluator();

        public SequenceSampler()
        {
            fontCollection.Install("assets/Font/arial.ttf");
            font         = fontCollection.CreateFont("arial", 72);
            PreviewImage = new Image<Rgba32>(30, 30);
        }

        public void Sample(Sequence sequence, int timeFrame)
        {
            var image = new Image<Rgba32>((int)sequence.Resolution.X, (int)sequence.Resolution.Y);

            image.Mutate(i => i.Fill(Color.Black));

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

                    if (_clipEvaluator.Evaluate(sequence, clip, timeFrame - clip.StartFrame))
                    {
                        image.Mutate(i => i.DrawImage(_clipEvaluator.ResultFrame.ImageSharpImage, 1f));
                    }
                }
            }

            image.Mutate(i => i.DrawText($"DEBUG FRAME RENDER: {timeFrame}", font, Color.Red, new PointF(0, 0)));

            PreviewImage = image;
        }
    }
}