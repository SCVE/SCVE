using System;
using System.IO;
using SCVE.Editor.Editing;
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

            for (var i = 0; i < sequence.Tracks.Count; i++)
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

                    if (clip is ImageClip imageClip)
                    {
                        var imageBytes     = File.ReadAllBytes(imageClip.ReferencedImageAsset.FileSystemFullPath);
                        var imageClipImage = Image.Load(imageBytes);

                        image.Mutate(i => i.DrawImage(imageClipImage, new Point(image.Width / 2 - imageClipImage.Width / 2, image.Height / 2 - imageClipImage.Height / 2),  1f));
                    }
                }
            }

            image.Mutate(i => i.DrawText($"DEBUG FRAME RENDER: {timeFrame}", font, Color.Red, new PointF(0, 0)));

            PreviewImage = image;
        }
    }
}