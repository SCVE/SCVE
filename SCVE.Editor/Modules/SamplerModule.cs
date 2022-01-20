namespace SCVE.Editor.Modules
{
    public class SamplerModule : IModule
    {
        public readonly SequenceSampler Sampler = new();

        public void Init()
        {
        }

        public void CrossReference(Modules modules)
        {
        }

        public void OnUpdate()
        {
        }
    }
}