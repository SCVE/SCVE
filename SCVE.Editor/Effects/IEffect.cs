namespace SCVE.Editor.Effects
{
    public interface IEffect
    {
        public ImageFrame Apply(EffectApplicationContext effectApplicationContext);

        public void OnImGuiRender();
    }
}