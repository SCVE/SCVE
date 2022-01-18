using SCVE.Editor.Editing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class ClipEvaluator
    {
        /// <summary>
        /// time must be in clip space
        /// </summary>
        public bool Evaluate(Sequence sequence, Clip clip, int time, ImageFrame clipResultImageFrame)
        {
            if (clip is EmptyClip)
            {
                // Don't evaluate any empty clips, because they produce no output
                return false;
            }
            else if (clip is ImageClip imageClip)
            {
                var imageAsset = Image.Load(imageClip.ReferencedImageAsset.FileSystemFullPath);

                clipResultImageFrame.ImageSharpImage.Mutate(i => i.DrawImage(imageAsset, 1));

                EffectApplicationContext effectApplicationContext = new EffectApplicationContext()
                {
                    Sequence         = sequence,
                    Clip             = clip,
                    SourceImageFrame = clipResultImageFrame
                };

                for (var i = 0; i < clip.Effects.Count; i++)
                {
                    clip.Effects[i].Apply(effectApplicationContext);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}