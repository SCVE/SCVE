using System;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.Services
{
    public class ClipEvaluator
    {
        private AssetCacheService _assetCacheService;
        private EditingService _editingService;
        private RenderingService _renderingService;

        public ClipEvaluator(AssetCacheService assetCacheService, EditingService editingService, RenderingService renderingService)
        {
            _assetCacheService = assetCacheService;
            _editingService = editingService;
            _renderingService = renderingService;
        }

        /// <summary>
        /// time must be in clip space
        /// </summary>
        public bool Evaluate(AssetClip clip, int time, byte[] pixels, int width, int height)
        {
            switch (clip.AssetType)
            {
                case AssetType.Sequence:
                    throw new NotImplementedException("Sequence Not Supported");
                case AssetType.Image:
                {
                    if (!_renderingService.CachedImages.ContainsKey(clip.ReferencedAssetId))
                    {
                        Console.WriteLine($"Cache miss found for asset: {clip.ReferencedAssetId}");
                        
                        _renderingService.LoadFromAssetClip(clip);
                    }
                    var cachedImage = _renderingService.CachedImages[clip.ReferencedAssetId];

                    ImageSharpImageManipulator.DrawOnTop(pixels, width, height, cachedImage.RgbaPixels, cachedImage.Width, cachedImage.Height);
                }
                    break;
                case AssetType.Folder:
                    throw new NotImplementedException("Folder Not Supported");
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
    }
}