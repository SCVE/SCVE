using SCVE.Editor.Effects;

namespace SCVE.Editor.Modules
{
    public class PreviewModule : IModule
    {
        private ImageFrame _displayedImage;

        public bool PreviewUpdateRequired { get; private set; }

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

        public void MarkPreviewDirty()
        {
            PreviewUpdateRequired = true;
        }

        public void ClearPreviewDirty()
        {
            PreviewUpdateRequired = false;
        }

        public void InvalidateSampledFrame(int index)
        {
            if (index == _editingModule.OpenedSequence.CursorTimeFrame)
            {
                PreviewImage = null;
            }

            PreviewCache.InvalidateSampledFrame(index);
        }

        public void OnUpdate()
        {
            if (PreviewUpdateRequired)
            {
                if (PreviewCache.Frames.ContainsKey(_editingModule.OpenedSequence.CursorTimeFrame))
                {
                    PreviewImage = PreviewCache.Frames[_editingModule.OpenedSequence.CursorTimeFrame];
                }
                else
                {
                    var sampledFrame = _samplerModule.Sampler.Sample(_editingModule.OpenedSequence, _editingModule.OpenedSequence.CursorTimeFrame);
                    sampledFrame.UploadGpuData();
                    PreviewCache.AddSampledFrame(_editingModule.OpenedSequence.CursorTimeFrame, sampledFrame);
                    PreviewImage = sampledFrame;
                }

                ClearPreviewDirty();
            }
        }
    }
}