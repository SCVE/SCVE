using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public enum ExportMode
    {
        PngSequence,
        AviUncompressed
    }

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

        private ImGuiSelectableContextMenu<Resolution> _resolutionContextMenu;
        private int _selectedResolutionIndex;

        private ImGuiEnumContextMenu<ExportMode> _exportModeContextMenu;
        private ExportMode _currentExportMode = ExportMode.PngSequence;

        public ExportSequenceModalPanel(ExportService exportService)
        {
            _exportService = exportService;
            _directoryPickerModalPanel = new DirectoryPickerModalPanel();
            _resolutionContextMenu = new ImGuiSelectableContextMenu<Resolution>(SupportedResolutions.Resolutions, 0, "##resolution");
            _exportModeContextMenu = new ImGuiEnumContextMenu<ExportMode>(0, "##export-mode");
            Name = "Export Sequence";
        }

        public void Open(Sequence sequence)
        {
            _sequence = sequence;
            _exportingFramesString = $"Exporting Frames: 0 - {sequence.FrameLength - 1}";
            _folderCreationInfoString = $"A folder \"{sequence.Title}\" will be created";
            _location = Environment.CurrentDirectory;
            _currentExportMode = ExportMode.PngSequence;
            _selectedResolutionIndex = 0;
            _isDone = false;
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

                _currentExportMode = _exportModeContextMenu.OnImGuiRender();

                switch (_currentExportMode)
                {
                    case ExportMode.PngSequence:
                        ImGui.Text(_folderCreationInfoString);
                        break;
                    case ExportMode.AviUncompressed:
                        ImGui.Text("Be aware of file size");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                ImGui.TextUnformatted("Select resolution");

                _selectedResolutionIndex = _resolutionContextMenu.OnImGuiRender();

                ImGui.Text(_exportingFramesString);

                if (_isExporting)
                {
                    unsafe
                    {
                        ImGuiNative.igProgressBar(_exportService.Progress, Vector2.Zero, null);
                    }

                    ImGui.TextDisabled("Export");
                    ImGui.TextDisabled("Close");
                }
                else
                {
                    if (ImGui.Button("Export"))
                    {
                        _isExporting = true;

                        switch (_currentExportMode)
                        {
                            case ExportMode.PngSequence:
                                Task.Run(() => { _exportService.ExportPngSequence(_sequence, SupportedResolutions.Resolutions[_selectedResolutionIndex].Value, _location); })
                                    .ContinueWith(_ =>
                                    {
                                        _isExporting = false;
                                        _isDone = true;
                                    });
                                break;
                            case ExportMode.AviUncompressed:
                                Task.Run(() => { _exportService.ExportAvi(_sequence, SupportedResolutions.Resolutions[_selectedResolutionIndex].Value, _location); })
                                    .ContinueWith(_ =>
                                    {
                                        _isExporting = false;
                                        _isDone = true;
                                    });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    if (_isDone)
                    {
                        ImGui.Text("Done!");
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