using System;
using SCVE.Editor.Editing;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Effects
{
    public interface IEffect
    {
        event Action Updated;
        
        public IImage Apply(EffectApplicationContext effectApplicationContext);

        public void OnImGuiRender();
        void AttachToClip(Clip clip);
        void DeAttachFromClip();
    }
}