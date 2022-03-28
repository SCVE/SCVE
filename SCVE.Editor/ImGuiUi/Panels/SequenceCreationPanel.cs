using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Late;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
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
        private int _fps = 0;
        private int _frameLength = 0;
        private int _resolution = 0;

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

                DrawSettings();
                DrawControls();

                ImGui.EndPopup();
            }
        }

        private void DrawSettings()
        {
            if (ImGui.InputText("Name", ref _name, 255))
            {
            }

            if (ImGui.DragInt("FPS", ref _fps, 0.1f, 10, 120))
            {
            }

            if (ImGui.DragInt("Frame length", ref _frameLength, 0.1f, 10, 60))
            {
            }

            if (ImGui.DragInt("Resolution", ref _resolution, 0.1f, 10, 120))
            {
            }
        }


        private void DrawControls()
        {
            if (ImGui.Button("Create"))
            {
                if (_name != string.Empty)
                {
                    var sequenceAsset = SequenceAsset.CreateNew(
                        name: _name,
                        location: _projectPanelService.CurrentLocation,
                        content: Sequence.CreateNew(30, new ScveVector2I(1280, 720), 150)
                    );

                    EditorApp.Late(new AddSequenceLateTask(sequenceAsset));

                    ImGui.CloseCurrentPopup();
                    Close();
                }
            }

            if (ImGui.Button("Close"))
            {
                ImGui.CloseCurrentPopup();
                Close();
            }
        }
    }
}