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
                    _settingsService.ShortenCursor(new(10, 10));
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