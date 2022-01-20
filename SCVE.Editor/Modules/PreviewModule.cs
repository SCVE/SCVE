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

        public void InvalidateSampledFrame(int index)
        {
            if (index == _editingModule.OpenedSequence.CursorTimeFrame)
            {
                PreviewImage = null;
            }

            PreviewCache.InvalidateSampledFrame(index);
            PreviewCurrentFrame();
        }

        public void PreviewCurrentFrame()
        {
            SetVisibleFrame(_editingModule.OpenedSequence.CursorTimeFrame);
        }

        public void SetVisibleFrame(int index)
        {
            if (PreviewCache.Frames.ContainsKey(index))
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

        public void OnUpdate()
        {
        }
    }
}