using ImGuiNET;
using SCVE.Editor.Abstractions;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class SequencePanel : IImGuiPanel
    {
        public SequencePanel()
        {
        }

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Sequence Panel"))
            {
                ImGui.End();
            }
        }
    }
}