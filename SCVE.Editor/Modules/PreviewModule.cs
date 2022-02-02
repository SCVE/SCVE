using SCVE.Editor.Effects;

namespace SCVE.Editor.Modules
{
    public class PreviewModule : IModule
    {
        public readonly InMemoryFrameCache PreviewCache = new();

        public ImageFrame PreviewImage;

        private EditingModule _editingModule;
        private SamplerModule _samplerModule;

        public void Init()
        {
        }

        public void CrossReference(Modules modules)
        {
            _editingModule = modules.Get<EditingModule>();
            _samplerModule = modules.Get<SamplerModule>();
        }

        public void InvalidateFrame(int index)
        {
            PreviewCache.InvalidateSampledFrame(index);
            if (index == _editingModule.OpenedSequence.CursorTimeFrame)
            {
                SyncVisiblePreview();
            }
        }

        /// <summary>
        /// Invalidates A range of sampled frames and performs preview sync if necessary
        /// </summary>
        public void InvalidateRange(int start, int length)
        {
            for (int i = start; i < start + length; i++)
            {
                PreviewCache.InvalidateSampledFrame(i);
            }

            var cursorTimeFrame = _editingModule.OpenedSequence.CursorTimeFrame;

            if(start <= cursorTimeFrame && cursorTimeFrame <= start + length)
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
            if (HasCached(index))
            {
                PreviewImage = PreviewCache.Frames[index];
            }
            else
            {
                var sampledFrame = _samplerModule.Sampler.Sample(_editingModule.OpenedSequence, index);
                PreviewCache.AddSampledFrame(index, sampledFrame);
                sampledFrame.UploadGpuData();
                PreviewImage = sampledFrame;
            }
        }

        public bool HasCached(int index)
        {
            return PreviewCache.Frames.ContainsKey(index);
        }

        public void OnUpdate()
        {
        }
    }
}