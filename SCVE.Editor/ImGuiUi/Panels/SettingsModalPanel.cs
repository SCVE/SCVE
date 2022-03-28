using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class SettingsModalPanel : ImGuiModalPanel
    {
        private SettingsService _settingsService;

        private Settings _draftSettings;

        private readonly Vector2 _windowSize = new(600, 350);

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

            ImGui.SetNextWindowSize(_windowSize);
            ImGui.SetNextWindowPos(ImGui.GetWindowViewport().Size / 2 - _windowSize / 2);
            if (ImGui.BeginPopupModal(Name, ref IsOpen, ImGuiWindowFlags.NoResize))
            {
                DrawSettings();
                DrawControls();

                ImGui.EndPopup();
            }
        }

        private void DrawSettings()
        {
            // NOTE: Expression call is not very efficient, but this is just a settings panel, not performance critical

            DrawSetting(s => s.CursorSize, 10, 20);
            DrawSetting(s => s.ClipPadding, 0, 10);
            DrawSetting(s => s.ClipRounding, 0, 10);
            DrawSetting(s => s.TrackHeight, 10, 30);
            DrawSetting(s => s.TrackMargin, 0, 10);
            DrawSetting(s => s.TrackMarginLeft, 0, 10);
            DrawSetting(s => s.TrackHeaderWidth, 50, 150);
            DrawSetting(s => s.SequenceHeaderHeight, 20, 50);
            DrawSetting(s => s.TimelineFrameMarkerHeight, 0, 10);
            DrawSetting(s => s.TimelineSecondsMarkerHeight, 0, 15);
        }

        private void DrawSetting(Expression<Func<Settings, int>> expression, int from, int to)
        {
            var memberExpression = (MemberExpression) expression.Body;
            string name = memberExpression.Member.Name;
            var value = expression.Compile().Invoke(_draftSettings);
            if (ImGui.DragInt(name, ref value, 0.1f, from, to))
            {
                (memberExpression.Member as PropertyInfo)!.SetValue(_draftSettings, value);
            }
        }

        private void DrawSetting(Expression<Func<Settings, float>> expression, float from, float to)
        {
            var memberExpression = (MemberExpression) expression.Body;
            string name = memberExpression.Member.Name;
            var value = expression.Compile().Invoke(_draftSettings);
            if (ImGui.DragFloat(name, ref value, 0.1f, from, to, "%.0f"))
            {
                (memberExpression.Member as PropertyInfo)!.SetValue(_draftSettings, value);
            }
        }

        private void DrawSetting(Expression<Func<Settings, Vector2>> expression, float from, float to)
        {
            var memberExpression = (MemberExpression) expression.Body;
            string name = memberExpression.Member.Name;
            var value = expression.Compile().Invoke(_draftSettings);
            if (ImGui.DragFloat2(name, ref value, 0.1f, from, to, "%.0f"))
            {
                (memberExpression.Member as PropertyInfo)!.SetValue(_draftSettings, value);
            }
        }

        private void DrawControls()
        {
            ImGui.SetCursorPos(new Vector2(8, _windowSize.Y - 20 - 8 - 4));

            if (ImGui.Button("Apply"))
            {
                EditorApp.Late("apply settings", () =>
                {
                    _settingsService.ApplySettings(_draftSettings);
                    Console.WriteLine("Settings have been applied.");
                });
            }

            ImGui.SameLine();

            if (ImGui.Button("Close"))
            {
                Console.WriteLine("Settings modal has been closed.");
                ImGui.CloseCurrentPopup();
                Close();
            }
        }
    }
}