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
        public bool Evaluate(AssetClip clip, int time, byte[] pixels, int width, int height)
        {
            return true;
        }
    }
}