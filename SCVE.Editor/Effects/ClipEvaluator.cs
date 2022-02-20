using SCVE.Editor.Editing;
using SCVE.Editor.MemoryUtils;
using SCVE.Editor.Modules;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using IImage = SCVE.Editor.Imaging.IImage;

namespace SCVE.Editor.Effects
{
    public class ClipEvaluator
    {
        /// <summary>
        /// time must be in clip space
        /// </summary>
        public bool Evaluate(Sequence sequence, Clip clip, int time, IImage clipResultImage)
        {
            if (clip is EmptyClip)
            {
                // Don't evaluate any empty clips, because they produce no output
                return false;
            }
            else if (clip is ImageClip imageClip)
            {
                var assetBytes = EditorApp.Modules.Get<AssetCacheModule>().Cache
                    .GetOrCache(imageClip.ReferencedImageAsset.FileSystemFullPath);
                using var imageAsset = Image.Load(assetBytes);

                using var clipResultImageSharpImage = Image.WrapMemory<Rgba32>(clipResultImage.ToByteArray(), clipResultImage.Width, clipResultImage.Height);

                clipResultImageSharpImage.Mutate(i => i.DrawImage(imageAsset, 1));

                EffectApplicationContext effectApplicationContext = new EffectApplicationContext()
                {
                    Sequence         = sequence,
                    Clip             = clip,
                    SourceImageFrame = clipResultImage
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