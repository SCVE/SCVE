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

        public void CrossReference(Modules modules)
        {
            _editingModule = modules.Get<EditingModule>();
            _samplerModule = modules.Get<SamplerModule>();
        }

        public void OnInit()
        {
            _previewCache = new ThreeWayCache(
                _editingModule.OpenedSequence?.FrameLength ?? 0,
                (int)(_editingModule.OpenedSequence?.Resolution.X ?? 1),
                (int)(_editingModule.OpenedSequence?.Resolution.Y ?? 1)
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
                var sampledFrame = _samplerModule.Sampler.Sample(_editingModule.OpenedSequence, index);
                _previewCache.Put(index, sampledFrame);
                _previewCache[index].ToGpu();
            }
        }

        public bool HasCached(int index)
        {
            return _previewCache.HasAnyPresence(index);
        }

        public void OnUpdate()
        {
        }
    }
}