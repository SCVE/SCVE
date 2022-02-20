using SCVE.Editor.Caching;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Modules
{
    public class PreviewService : IService
    {
        private ThreeWayCache _previewCache;

        public ThreeWayImage PreviewImage => _previewCache[_editingService.OpenedSequence.CursorTimeFrame];

        private readonly EditingService _editingService;
        private readonly SamplerService _samplerService;

        public PreviewService(EditingService editingService, SamplerService samplerService)
        {
            _editingService = editingService;
            _samplerService = samplerService;
            
            _previewCache = new ThreeWayCache(
                _editingService.OpenedSequence.FrameLength,
                (int)_editingService.OpenedSequence.Resolution.X,
                (int)_editingService.OpenedSequence.Resolution.Y
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
            SetVisibleFrame(_editingService.OpenedSequence.CursorTimeFrame);
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