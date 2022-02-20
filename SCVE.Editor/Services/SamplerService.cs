namespace SCVE.Editor.Services
{
    public class SamplerService : IService
    {
        public readonly SequenceSampler Sampler;

        public SamplerService(SequenceSampler sampler)
        {
            Sampler = sampler;
        }

        public void OnUpdate()
        {
        }
    }
}