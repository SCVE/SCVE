using System;
using System.Collections.Generic;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;
using Silk.NET.OpenAL;

namespace SCVE.Editor.ImGuiUi
{
    public class SequenceCreationPanel : IImGuiRenderable
    {
        private readonly EditingService _editingService;

        public SequenceCreationPanel(EditingService editingService)
        {
            _editingService = editingService;
        }

        public bool Visible { get; set; }


        private string _name = "";

        public void OnImGuiRender()
        {
            if (!Visible)
            {
                return;
            }

            ImGui.OpenPopup("New Sequence");
            bool _visible = Visible;


            // Checks if the popup modal "New Sequence" is opened.
            if (ImGui.BeginPopupModal("New Sequence", ref _visible))
            {
                Visible = _visible;
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
                            Location = "/",
                            Content = Sequence.CreateNew(30, new ScveVector2i(1280, 720), 150)
                        };

                        _editingService.AddSequence(newSequence);

                        ImGui.CloseCurrentPopup();
                        Visible = false;
                    }
                }

                if (ImGui.Button("Close"))
                {
                    Visible = false;
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }
            else
            {
                Visible = false;
                ImGui.CloseCurrentPopup();
            }
        }
    }
}