using ImGuiNET;
using SCVE.Editor.Editing.Effects;
using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.ImGuiUi
{
    public class EffectImGuiRenderer : IEffectVisitor
    {
        public void Visit(EffectBase effect)
        {
            effect.AcceptVisitor(this);
        }

        public void Visit(Translate effect)
        {
            int x = effect.X;
            if (ImGui.SliderInt("X", ref x, -1000, 1000))
            {
                effect.X = x;
                effect.InvokeUpdated();
            }

            int y = effect.Y;
            if (ImGui.SliderInt("Y", ref y, -1000, 1000))
            {
                effect.Y = y;
                effect.InvokeUpdated();
            }
        }

        public void Visit(Scale effect)
        {
            float x = effect.X;
            if (ImGui.SliderFloat("X", ref x, 0, 5))
            {
                effect.X = x;
                effect.InvokeUpdated();
            }

            float y = effect.Y;
            if (ImGui.SliderFloat("Y", ref y, 0, 5))
            {
                effect.Y = y;
                effect.InvokeUpdated();
            }
        }
    }
}