using System;
using System.IO;
using System.Linq;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Engine.ImageSharpBindings;
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
            switch (clip.AssetType)
            {
                case AssetType.Sequence:
                    throw new NotImplementedException("Sequence Not Supported");
                    break;
                case AssetType.Image:
                {
                    var imageAsset = _editingService.OpenedProject.Images.First(a => a.Guid == clip.ReferencedAssetId);

                    var textureFileData = ImageSharpTextureLoader.Load(imageAsset.Content.RelativePath);

                    ImageSharpImageManipulator.DrawOnTop(pixels, width, height, textureFileData.RgbaPixels, textureFileData.Width, textureFileData.Height);
                }
                    break;
                case AssetType.Folder:
                    throw new NotImplementedException("Folder Not Supported");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }
    }
}