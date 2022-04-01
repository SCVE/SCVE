using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ExportSequenceModalPanel : ImGuiModalPanel
    {
        private string _exportingFramesString;
        private string _folderCreationInfoString;
        private Sequence _sequence;

        private bool _isExporting = false;
        private string _location;
        private readonly DirectoryPickerModalPanel _directoryPickerModalPanel;

        private ExportService _exportService;
        private bool _isDone;

        public ExportSequenceModalPanel(ExportService exportService)
        {
            _exportService = exportService;
            _directoryPickerModalPanel = new DirectoryPickerModalPanel();
            Name = "Export Sequence";
        }

        public void Open(Sequence sequence)
        {
            _sequence = sequence;
            _exportingFramesString = $"Exporting Frames: 0 - {sequence.FrameLength - 1}";
            _folderCreationInfoString = $"A folder \"{sequence.Title}\" will be created";
            _location = Environment.CurrentDirectory;
            IsOpen = true;
        }

        public override void OnImGuiRender()
        {
            if (IsOpen)
            {
                ImGui.OpenPopup(Name);
            }

            ImGui.SetNextWindowSize(new Vector2(600, 400));
            if (ImGui.BeginPopupModal(Name, ref IsOpen, ImGuiWindowFlags.NoResize))
            {
                ImGui.TextDisabled($"Export Sequence");

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

                ImGui.Text(_folderCreationInfoString);

                ImGui.Text(_exportingFramesString);

                if (_isExporting)
                {
                    unsafe
                    {
                        ImGuiNative.igProgressBar(_exportService.Progress, Vector2.Zero, (byte*) null);
                    }

                    ImGui.TextDisabled("Export");
                    ImGui.TextDisabled("Close");
                }
                else
                {
                    if (_isDone)
                    {
                        ImGui.Text("Done!");
                    }
                    else
                    {
                        if (ImGui.Button("Export"))
                        {
                            _isExporting = true;

                            Task.Run(() => { _exportService.Export(_sequence, _location); })
                                .ContinueWith(_ =>
                                {
                                    _isExporting = false;
                                    _isDone = true;
                                });
                        }
                    }

                    if (ImGui.Button("Close"))
                    {
                        ImGui.CloseCurrentPopup();
                        Close();
                    }
                }

                ImGui.EndPopup();
            }
        }
    }
}