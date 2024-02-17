using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.ImGuiUi.Panels;

namespace SCVE.Editor.ImGuiUi
{
    public class MainMenuBar : IImGuiPanel
    {
        private readonly ProjectCreationPanel _projectCreationPanel;

        public MainMenuBar(ProjectCreationPanel projectCreationPanel)
        {
            _projectCreationPanel = projectCreationPanel;
        }

        public void OnImGuiRender()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New Project", "Ctrl+N"))
                    {
                        _projectCreationPanel.Open();
                    }

                    if (ImGui.MenuItem("Open Project", "Ctrl+O"))
                    {
                    }

                    if (ImGui.BeginMenu("Open Recent"))
                    {
                        ImGui.EndMenu();
                    }

                    if (ImGui.MenuItem("Exit"))
                    {
                        EditorApp.Instance.Exit();
                    }

                    ImGui.EndMenu();
                }
                
                if (ImGui.BeginMenu("Windows"))
                {
                    if (ImGui.MenuItem("Settings"))
                    {
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }

        // This is a direct port of imgui_demo.cpp HelpMarker function

        // https://github.com/ocornut/imgui/blob/master/imgui_demo.cpp#L190

        private void ShowHint(string message)
        {
            // ImGui.TextDisabled("(?)");
            if (ImGui.IsItemHovered())
            {
                // Change background transparency
                ImGui.PushStyleColor(ImGuiCol.PopupBg, new Vector4(1, 1, 1, 0.8f));
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted(message);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
                ImGui.PopStyleColor();
            }
        }
    }
}