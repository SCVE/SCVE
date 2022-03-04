using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;
using SixLabors.ImageSharp.PixelFormats;

namespace SCVE.Editor.Services
{
    public class ClipEvaluator
    {
        private AssetCacheService _assetCacheService;
        private EditingService _editingService;

        public ClipEvaluator(AssetCacheService assetCacheService, EditingService editingService)
        {
            _assetCacheService = assetCacheService;
            _editingService = editingService;
        }


        /// <summary>
        /// time must be in clip space
        /// </summary>
        public bool Evaluate(Clip clip, int time, byte[] pixels, int width, int height)
        {
            if (clip is EmptyClip)
            {
                // Don't evaluate any empty clips, because they produce no output
                return false;
            }
            else if (clip is ImageClip imageClip)
            {
                
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}