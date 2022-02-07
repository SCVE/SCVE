using SCVE.Editor.Caching;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Modules
{
    public class PreviewModule : IModule
    {
        private ThreeWayCache _previewCache;

        public ThreeWayImage PreviewImage => _previewCache[_editingModule.OpenedSequence.CursorTimeFrame];

        private EditingModule _editingModule;
        private SamplerModule _samplerModule;

        public void CrossReference(ModulesContainer modulesContainer)
        {
            _editingModule = modulesContainer.Get<EditingModule>();
            _samplerModule = modulesContainer.Get<SamplerModule>();
        }

        public void OnInit()
        {
            _previewCache = new ThreeWayCache(
                _editingModule.OpenedSequence.FrameLength,
                (int)_editingModule.OpenedSequence.Resolution.X,
                (int)_editingModule.OpenedSequence.Resolution.Y
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

            var cursorTimeFrame = _editingModule.OpenedSequence.CursorTimeFrame;

            if (start <= cursorTimeFrame && cursorTimeFrame <= start + length)
            {
                SyncVisiblePreview();
            }
        }

        public void SyncVisiblePreview()
        {
            SetVisibleFrame(_editingModule.OpenedSequence.CursorTimeFrame);
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
            var sampledFrame = _samplerModule.Sampler.Sample(_editingModule.OpenedSequence, index);
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
            for (int i = 0; i < _editingModule.OpenedSequence.FrameLength; i++)
            {
                RenderFrame(i);
            }
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