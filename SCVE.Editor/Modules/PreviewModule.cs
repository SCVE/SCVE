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
                PreviewImage = null;
                RenderCurrentFrame();
            }
        }

        public void InvalidateRange(int start, int length)
        {
            for (int i = start; i < start + length; i++)
            {
                PreviewCache.InvalidateSampledFrame(i);
            }
            
            if(_editingModule.OpenedSequence.CursorTimeFrame.IsWithinInclusive(start, start + length - 1))
            {
                RenderCurrentFrame();
            }
        }

        public void RenderCurrentFrame()
        {
            SetVisibleFrame(_editingModule.OpenedSequence.CursorTimeFrame);
        }

        public void SetVisibleFrame(int index)
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