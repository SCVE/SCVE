using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SettingsModalPanel : ImGuiModalPanel
    {
        private SettingsService _settingsService;
        
        public SettingsModalPanel(SettingsService settingsService)
        {
            _settingsService = settingsService;
            Name = "Settings";
        }

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }

            ImGui.SetNextWindowSize(new Vector2(400, 350));
            if (ImGui.BeginPopupModal(Name, ref IsOpen))
            {
                if (ImGui.Button("Shorten cursor"))
                {
                    // TODO: Cursor does not become shorter by Y axis. Repair later, luv.
                    _settingsService.SettingsInstance.CursorSize = new(10, 10);
                }
                
                if (ImGui.Button("Save"))
                {
                    _settingsService.TrySave();
                    
                    Console.WriteLine("Settings have been saved.");
                    ImGui.CloseCurrentPopup();
                    Close();
                }

                if (ImGui.Button("Close"))
                {
                    Console.WriteLine("Settings modal has been closed.");
                    ImGui.CloseCurrentPopup();
                    Close();
                }

                ImGui.EndPopup();
            }
        }
    }
}