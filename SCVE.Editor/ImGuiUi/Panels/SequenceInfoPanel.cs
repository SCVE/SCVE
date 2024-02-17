using ImGuiNET;
using SCVE.Editor.Abstractions;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class SequenceInfoPanel : IImGuiPanel
    {
        public SequenceInfoPanel()
        {
        }

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Sequence Info Panel"))
            {
                ImGui.End();
            }
        }
    }
}