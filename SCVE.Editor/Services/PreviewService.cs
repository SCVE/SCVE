using System.Numerics;
using SCVE.Editor.Caching;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Services
{
    public class PreviewService : IService
    {
        private ThreeWayCache _previewCache;

        public ThreeWayImage PreviewImage { get; private set; }

        private ThreeWayImage _noPreviewImage;

        private readonly EditingService _editingService;
        private readonly SamplerService _samplerService;

        private static Vector2 _previewResolution = new(1280, 720);

        public PreviewService(EditingService editingService, SamplerService samplerService)
        {
            _editingService = editingService;
            _samplerService = samplerService;
            
            _previewCache = new ThreeWayCache(
                1,
                (int)_previewResolution.X,
                (int)_previewResolution.Y
            );
        }

        /// <summary>
        /// Invalidates A range of sampled frames and performs preview sync if necessary
        /// </summary>
        public void InvalidateRange(int start, int length)
        {
            for (int i = start; i < start + length; i++)
            {
                _previewCache.Invalidate(i);
            }

            var cursorTimeFrame = _editingService.OpenedSequence.CursorTimeFrame;

            if (start <= cursorTimeFrame && cursorTimeFrame <= start + length)
            {
                SyncVisiblePreview();
            }
        }

        public void SyncVisiblePreview()
        {
            if (_editingService.OpenedSequence is null)
            {
                _previewCache.ForceReplace(0, _noPreviewImage ??= Utils.CreateNoPreviewImage((int) _previewResolution.X, (int) _previewResolution.Y));
                _previewCache[0].ToGpu();
                PreviewImage = _previewCache[0];
            }
            else
            {
                SetVisibleFrame(_editingService.OpenedSequence.CursorTimeFrame);
            }
        }

        private void SetVisibleFrame(int index)
        {
            if (HasCached(index)) return;

            if (_previewCache.TryMakeFromDisk(index))
            {
                _previewCache[index].ToGpu();
            }
            else
            {
                RenderFrame(index);
            }
            PreviewImage = _previewCache[index];
        }

        public void RenderFrame(int index)
        {
            var sampledFrame = _samplerService.Sampler.Sample(_editingService.OpenedSequence, index);
            _previewCache.ForceReplace(index, sampledFrame);
            _previewCache[index].ToGpu();
        }

        public void RenderRange(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                RenderFrame(i);
            }
        }

        public void RenderSequence()
        {
            RenderRange(0, _editingService.OpenedSequence.FrameLength);
        }

        public bool HasCached(int index)
        {
            return _previewCache.HasAnyPresence(index);
        }
        
        public bool HasCached(int index, ImagePresence presence)
        {
            return _previewCache[index].Presence == presence;
        }

        public void OnUpdate()
        {
        }
    }
}