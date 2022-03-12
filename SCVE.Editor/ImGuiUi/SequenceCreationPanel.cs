using System;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SequenceCreationPanel : ImGuiModalPanel
    {
        private readonly EditingService _editingService;
        private readonly ProjectPanelService _projectPanelService;

        public SequenceCreationPanel(EditingService editingService, ProjectPanelService projectPanelService)
        {
            _editingService = editingService;
            _projectPanelService = projectPanelService;
            Name = "New Sequence";
        }

        private string _name = "";

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }

            // Checks if the popup modal "New Sequence" is opened.
            if (ImGui.BeginPopupModal(Name, ref IsOpen))
            {
                ImGui.TextDisabled($"New sequence");

                if (ImGui.InputText("Name", ref _name, 255))
                {
                }

                if (ImGui.Button("Create"))
                {
                    if (_name != string.Empty)
                    {
                        var newSequence = new SequenceAsset()
                        {
                            Guid = Guid.NewGuid(),
                            Name = _name,
                            Location = _projectPanelService.CurrentLocation,
                            Content = Sequence.CreateNew(30, new ScveVector2i(1280, 720), 150)
                        };

                        _editingService.AddSequence(newSequence);

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