using System;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ProjectCreationPanel : ImGuiModalPanel
    {
        private EditingService _editingService;
        private ModalManagerService _modalManagerService;
        private RecentsService _recentsService;
        private ProjectPanelService _projectPanelService;
        private DirectoryPickerModalPanel _directoryPickerModalPanel;

        public ProjectCreationPanel(EditingService editingService, RecentsService recentsService, ProjectPanelService projectPanelService)
        {
            _editingService = editingService;
            _recentsService = recentsService;
            _projectPanelService = projectPanelService;
            _directoryPickerModalPanel = new DirectoryPickerModalPanel();
            Name = "New Project";
        }

        private string _title = "";

        private string _location = Environment.CurrentDirectory;

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }

            ImGui.SetNextWindowSize(new Vector2(600, 400));
            if (ImGui.BeginPopupModal(Name, ref IsOpen, ImGuiWindowFlags.NoResize))
            {
                ImGui.TextDisabled($"New Project");

                ImGui.InputText("Title", ref _title, 256);
                ImGui.Separator();

                ImGui.TextDisabled("Location");
                ImGui.TextDisabled(_location);

                ImGui.SameLine();

                if (ImGui.Button("Choose location"))
                {
                    _directoryPickerModalPanel.Open(Environment.CurrentDirectory, "Choose location");
                }

                string location = "";
                if (_directoryPickerModalPanel.OnImGuiRender(ref location))
                {
                    _location = location;
                }

                if (_title.Length == 0)
                {
                    ImGui.TextDisabled("Create");
                }
                else
                {
                    if (ImGui.Button("Create"))
                    {
                        var videoProject = VideoProject.CreateNew(_title);
                        var projectPath = Path.Combine(_location, $"{_title}.scveproject");
                        Utils.WriteJson(videoProject, projectPath);
                        Console.WriteLine($"Created project {_title}");

                        EditorApp.Late("open project", () =>
                        {
                            _editingService.SetOpenedProject(videoProject);

                            _recentsService.NoticeOpen(projectPath);
                            _projectPanelService.ChangeLocation("/");
                        });

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