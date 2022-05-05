using System;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Imaging;
using SCVE.Editor.Services;

namespace SCVE.Editor.Background
{
    public class RenderFrameBackgroundJob : BackgroundJob<ThreeWayImage>
    {
        private Sequence _sequence;
        private readonly SamplerService _samplerService;

        private readonly ScveVector2I _resolution;

        private int _frame;

        public RenderFrameBackgroundJob(Sequence sequence, SamplerService samplerService, ScveVector2I resolution, int frame, Action<ThreeWayImage> onFinished) : base(onFinished)
        {
            _sequence = sequence;
            _samplerService = samplerService;
            _resolution = resolution;
            _frame = frame;
        }

        public override void Run()
        {
            Result = _samplerService.Sample(_sequence, _resolution, _frame);
        }
    }
}