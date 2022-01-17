using SCVE.Editor.Editing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SCVE.Editor.Effects
{
    public class ClipEvaluator
    {
        public ImageFrame ResultFrame { get; set; }
        
        /// <summary>
        /// time must be in clip space
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="time"></param>
        public bool Evaluate(Sequence sequence, Clip clip, int time)
        {
            if (clip is EmptyClip)
            {
                // Don't evaluate any empty clips, because they produce no output
                return false;
            }
            else if (clip is ImageClip imageClip)
            {
                ImageFrame frame = new ImageFrame((int)sequence.Resolution.X, (int)sequence.Resolution.Y);
                frame.CreateImageSharpWrapper();

                var imageAsset = Image.Load(imageClip.ReferencedImageAsset.FileSystemFullPath);
                
                frame.ImageSharpImage.Mutate(i => i.DrawImage(imageAsset, 1));
                EffectApplicationContext effectApplicationContext = new EffectApplicationContext()
                {
                    Sequence = sequence,
                    Clip     = clip,
                    ImageFrame = frame
                };
                
                for (var i = 0; i < clip.Effects.Count; i++)
                {
                    effectApplicationContext.ImageFrame = clip.Effects[i].Apply(effectApplicationContext);
                }

                ResultFrame = effectApplicationContext.ImageFrame;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}