using SCVE.Editor.Editing.Visitors;

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

        public abstract void AcceptVisitor(IEffectVisitor visitor);

        public void InvokeUpdated()
        {
            Updated?.Invoke();
        }
    }
}