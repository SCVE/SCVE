namespace SCVE.Editor.Modules
{
    public class SamplerModule : IModule
    {
        public readonly SequenceSampler Sampler = new();

        public void OnInit()
        {
        }

        public void CrossReference(ModulesContainer modulesContainer)
        {
        }

        public void OnUpdate()
        {
        }
    }
}