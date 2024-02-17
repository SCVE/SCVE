using ImGuiNET;
using SCVE.Editor.Abstractions;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ProjectPanel : IImGuiPanel
    {
        public void OnImGuiRender()
        {
            if (ImGui.Begin("Project Panel", ImGuiWindowFlags.MenuBar))
            {
                if (EditorApp.Instance.ActiveProject is null)
                {
                    ImGui.Text("No project is opened");
                }

                ImGui.End();
            }
        }
    }
}