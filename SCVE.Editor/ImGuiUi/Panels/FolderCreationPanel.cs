using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Late;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class FolderCreationPanel : ImGuiModalPanel
    {
        private readonly EditingService _editingService;
        private readonly ProjectPanelService _projectPanelService;

        public FolderCreationPanel(EditingService editingService, ProjectPanelService projectPanelService)
        {
            _editingService = editingService;
            _projectPanelService = projectPanelService;
            Name = "New Folder";
        }

        private string _name = "";

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }

            // Checks if the popup modal "New Folder" is opened.
            if (ImGui.BeginPopupModal(Name, ref IsOpen))
            {
                ImGui.TextDisabled($"New folder");

                if (ImGui.InputText("Name", ref _name, 255))
                {
                }

                if (ImGui.Button("Create"))
                {
                    if (_name != string.Empty)
                    {
                        var folderAsset = FolderAsset.CreateNew(
                            name: _name,
                            location: _projectPanelService.CurrentLocation,
                            content: new Folder()
                        );

                        
                        EditorApp.Late(new AddFolderLateTask(folderAsset));

                        ImGui.CloseCurrentPopup();
                        Close();
                    }
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