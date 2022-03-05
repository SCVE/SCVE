using SCVE.Editor.Editing.Effects;

namespace SCVE.Editor.Editing.Visitors
{
    public interface IEffectVisitor
    {
        void Visit(EffectBase effect);
        
        void Visit(TranslateEffect effect);

        void Visit(ScaleEffect effect);
    }
}