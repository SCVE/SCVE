namespace SCVE.Editor.Effects
{
    public interface IEffect
    {
        public string VisibleName { get; }
        public ImageFrame Apply(EffectApplicationContext effectApplicationContext);

        public void OnImGuiRender();
    }
}