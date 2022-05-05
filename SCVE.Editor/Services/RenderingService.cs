using System;
using System.Collections.Generic;
using System.Linq;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Engine.Core.Loading;
using SCVE.Engine.Core.Misc;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.Services
{
    public class RenderingService : IService
    {
        private Dictionary<Guid, TextureFileData> _cachedImages = new();
        public IReadOnlyDictionary<Guid, TextureFileData> CachedImages => _cachedImages;

        private EditingService _editingService;

        public RenderingService(EditingService editingService)
        {
            _editingService = editingService;
        }

        /// <summary>
        /// Loads all assets from disk for bulk render
        /// </summary>
        public void BuildAssetCache(Sequence sequence, int startFrame, int length)
        {
            Console.WriteLine($"Preparing rendering sequence({sequence.Title}): from {startFrame}, length {length}");
            _cachedImages.Clear();

            foreach (var track in sequence.Tracks)
            {
                foreach (var assetClip in track.AssetClips)
                {
                    // either start of the clip, or it's end is in requested bounds
                    if (assetClip.StartFrame >= startFrame && assetClip.StartFrame < startFrame + length ||
                        assetClip.StartFrame + assetClip.FrameLength >= startFrame && assetClip.StartFrame + assetClip.FrameLength < startFrame + length)
                    {
                        LoadFromAssetClip(assetClip);
                    }
                }
            }
        }

        public void LoadFromAssetClip(AssetClip clip)
        {
            switch (clip.AssetType)
            {
                case AssetType.Sequence:
                    break;
                case AssetType.Image:
                    var imageAsset = _editingService.OpenedProject.Images.FirstOrDefault(i => i.Guid == clip.ReferencedAssetId);

                    if (imageAsset is null)
                    {
                        throw new ScveException("Image asset not found");
                    }
                                
                    // TODO: RAM CRITICAL.
                    // let's assume for now, that there is enough RAM to load all referenced images

                    var textureFileData = ImageSharpTextureLoader.Load(imageAsset.Content.RelativePath);
                    _cachedImages[imageAsset.Guid] = textureFileData;

                    break;
                case AssetType.Folder:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void PurgeAssetCache()
        {
            _cachedImages.Clear();
        }
    }
}