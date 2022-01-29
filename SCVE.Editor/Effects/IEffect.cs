using SCVE.Editor.Editing;

namespace SCVE.Editor.Effects
{
    public interface IEffect
    {
        public ImageFrame Apply(EffectApplicationContext effectApplicationContext);

        public void OnImGuiRender();
        void AttachToClip(Clip clip);
        void DeAttachFromClip();
    }
}