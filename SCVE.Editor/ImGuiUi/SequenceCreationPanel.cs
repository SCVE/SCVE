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

        const uint BufferSize = 255;
        byte[] Buffer = new byte[BufferSize];

        public SequenceCreationPanel(EditingService editingService)
        {
            _editingService = editingService;
        }

        public bool Visible { get; set; }

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

                if (ImGui.InputText("Name", Buffer, BufferSize))
                {
                }


                if (ImGui.Button("Create"))
                {
                    //TODO: Bug with clearing input but saving in Buffer the previous value without the first char only.
                    
                    var sequenceName = System.Text.Encoding.UTF8.GetString(Buffer).Trim('\0');

                    if (sequenceName != string.Empty)
                    {
                        var newSequence = new SequenceAsset()
                        {
                            Guid = Guid.NewGuid(),
                            Name = sequenceName,
                            Location = "/",
                            Content = new Sequence()
                            {
                                Guid = Guid.NewGuid(),
                                Resolution = new ScveVector2i(1280, 720),
                                Tracks = new List<Track>(0),
                                FrameLength = 150,
                                FPS = 30,
                            }
                        };
                        
                        _editingService.AddSequence(newSequence);
                        
                        Array.Clear(Buffer);
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