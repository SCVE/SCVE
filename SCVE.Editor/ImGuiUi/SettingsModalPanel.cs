using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SettingsModalPanel : ImGuiModalPanel
    {
        private SettingsService _settingsService;

        private Settings _draftSettings;

        public void LoadDraft()
        {
            _draftSettings = Settings.GetClone();
        }

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
            if (ImGui.BeginPopupModal(Name, ref IsOpen, ImGuiWindowFlags.NoResize))
            {
                DrawSettings();
                DrawControls();


                ImGui.EndPopup();
            }
        }

        private void DrawControls()
        {
            ImGui.SetCursorPos(new Vector2(8, 350 - 20 - 8 - 4));

            if (ImGui.Button("Apply"))
            {
                _settingsService.ApplySettings(_draftSettings);
                Console.WriteLine("Settings have been applied.");
            }

            ImGui.SameLine();

            if (ImGui.Button("Close"))
            {
                Console.WriteLine("Settings modal has been closed.");
                ImGui.CloseCurrentPopup();
                Close();
            }
        }

        private void DrawSettings()
        {
            if (ImGui.Button("Shorten cursor"))
            {
                _draftSettings.CursorSize = new Vector2(10, 10);
            }

            if (ImGui.Button("Largen cursor"))
            {
                _draftSettings.CursorSize = new Vector2(10, 20);
            }
        }
    }
}