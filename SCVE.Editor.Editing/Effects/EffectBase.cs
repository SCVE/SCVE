namespace SCVE.Editor.Editing.Effects
{
    public abstract class EffectBase
    {
        public event Action Updated;
        
        protected abstract void Algorithm(byte[] pixels, int width, int height);

        public void Apply(byte[] pixels, int width, int height)
        {
            Algorithm(pixels, width, height);
        }
        
        protected abstract void OnImGuiRenderAlgorithm();

        public void OnImGuiRender()
        {
            OnImGuiRenderAlgorithm();
        }
    }
}