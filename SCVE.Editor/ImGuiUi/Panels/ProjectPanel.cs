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
                var project = EditorApp.Instance.ActiveProject;
                if (project is null)
                {
                    ImGui.Text("No project is opened");
                }
                else
                {
                    ImGui.Text(project.Title);
                }

                ImGui.End();
            }
        }
    }
}