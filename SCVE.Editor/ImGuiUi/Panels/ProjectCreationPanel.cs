using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ProjectCreationPanel : ImGuiModalPanel
    {
        private EditingService _editingService;
        
        public ProjectCreationPanel(EditingService editingService)
        {
            _editingService = editingService;
            Name = "New Project";
        }

        private string _title = "";

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }
            
            if (ImGui.BeginPopupModal(Name, ref IsOpen))
            {
                ImGui.TextDisabled($"New Project");

                if (ImGui.InputText("Title", ref _title, 255))
                {
                }

                if (ImGui.Button("Create"))
                {
                    var videoProject = VideoProject.CreateNew(_title);
                    
                    _editingService.SetOpenedProject(videoProject);
                    
                    ImGui.CloseCurrentPopup();
                    Close();
                }

                if (ImGui.Button("Close"))
                {
                    ImGui.CloseCurrentPopup();
                    Close();
                }

                ImGui.EndPopup();
            }
        }
    }
}