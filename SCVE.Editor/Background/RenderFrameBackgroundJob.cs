using System;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Background
{
    public class RenderFrameBackgroundJob : BackgroundJob<ThreeWayImage>
    {
        private Sequence _sequence;

        private SequenceSampler _sampler;
        private readonly ScveVector2i _resolution;

        private int _frame;

        public RenderFrameBackgroundJob(Sequence sequence, SequenceSampler sampler, ScveVector2i resolution, int frame, Action<ThreeWayImage> onFinished) : base(onFinished)
        {
            _sequence = sequence;
            _sampler = sampler;
            _resolution = resolution;
            _frame = frame;
        }

        public override void Run()
        {
            Result = _sampler.Sample(_sequence, _resolution, _frame);
        }
    }
}