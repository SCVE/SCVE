using System;
using System.IO;
using System.Numerics;
using System.Text.Json;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class MainMenuBar : IImGuiRenderable
    {
        private PreviewService _previewService;
        private EditingService _editingService;

        private ModalManagerService _modalManagerService;

        private RecentsService _recentsService;

        public MainMenuBar(PreviewService previewService, EditingService editingService,
            ModalManagerService modalManagerService, RecentsService recentsService)
        {
            _previewService = previewService;
            _editingService = editingService;
            _modalManagerService = modalManagerService;
            _recentsService = recentsService;
        }

        public void OnImGuiRender()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    // Disabling fullscreen would allow the window to be moved to the front of other windows, 
                    // which we can't undo at the moment without finer window depth/z control.
                    //ImGui::MenuItem("Fullscreen", NULL, &opt_fullscreen_persistant);1
                    if (ImGui.MenuItem("New Project", "Ctrl+N"))
                    {
                        _modalManagerService.OpenProjectCreationPanel();
                    }

                    if (ImGui.MenuItem("Open Project", "Ctrl+O"))
                    {
                        _modalManagerService.OpenFilePickerPanel(Environment.CurrentDirectory, () =>
                        {
                            string path = _modalManagerService.FilePickerSelectedPath;

                            if (Path.GetExtension(path) == ".scveproject")
                            {
                                var jsonContent = File.ReadAllText(path);

                                var videoProject = JsonSerializer.Deserialize<VideoProject>(jsonContent,
                                    new JsonSerializerOptions()
                                    {
                                        PropertyNameCaseInsensitive = true
                                    });

                                _editingService.SetOpenedProject(videoProject, path);
                                _previewService.SyncVisiblePreview();
                                _recentsService.NoticeOpenNew(path);

                                Console.WriteLine($"Loaded project: {videoProject.Title}");
                            }
                            else
                            {
                                Console.WriteLine($"Unknown file selected: {Path.GetExtension(path)}");
                            }
                        }, () => { Console.WriteLine("Opening file dialog was dismissed"); });
                    }

                    if (_recentsService.Recents.Count != 0)
                    {
                        if (ImGui.BeginMenu("Open Recent"))
                        {
                            foreach (var recent in _recentsService.Recents)
                            {
                                var fileName = Path.GetFileName(recent);

                                if (ImGui.MenuItem(fileName))
                                {
                                    var jsonContent = File.ReadAllText(recent);

                                    var videoProject = JsonSerializer.Deserialize<VideoProject>(jsonContent,
                                        new JsonSerializerOptions()
                                        {
                                            PropertyNameCaseInsensitive = true
                                        });

                                    _editingService.SetOpenedProject(videoProject, recent);
                                    _previewService.SyncVisiblePreview();
                                    _recentsService.NoticeOpenRecent(recent);

                                    Console.WriteLine($"Loaded Recent Project: {videoProject.Title}");
                                    
                                    // NOTE: break is needed, because _recentsService.NoticeOpenRecent() modifies original list
                                    break;
                                }

                                ShowHint(recent);
                            }

                            ImGui.EndMenu();
                        }
                    }

                    if (_editingService.OpenedProject is not null)
                    {
                        if (ImGui.MenuItem("Save Project", "Ctrl+Shift+S"))
                        {
                            var jsonContent = JsonSerializer.Serialize(_editingService.OpenedProject,
                                new JsonSerializerOptions()
                                {
                                    PropertyNameCaseInsensitive = true,
                                    WriteIndented = true
                                });

                            File.WriteAllText("testdata/savetest.scveproject", jsonContent);
                        }
                    }

                    if (ImGui.MenuItem("Exit"))
                    {
                        EditorApp.Instance.Exit();
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Sequence"))
                {
                    if (ImGui.MenuItem("Render start to end", "Ctrl+R"))
                    {
                        _previewService.RenderSequence();
                    }

                    if (ImGui.MenuItem("Add Track"))
                    {
                        if (_editingService.OpenedProject is not null)
                        {
                            _editingService.OpenedSequence.Tracks.Add(Track.CreateNew());
                        }
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Assets"))
                {
                    if (ImGui.MenuItem("Add Image"))
                    {
                        if (_editingService.OpenedProject is not null)
                        {
                            _modalManagerService.OpenFilePickerPanel(Environment.CurrentDirectory, () =>
                            {
                                string path = _modalManagerService.FilePickerSelectedPath;

                                var fileName = Path.GetFileName(path);
                                var extension = Path.GetExtension(path);
                                var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, path);
                                if (extension is ".jpeg" or ".png")
                                {
                                    _editingService.OpenedProject.AddImage(fileName!, relativePath);
                                }
                                else
                                {
                                    Console.WriteLine($"Unknown file selected: {extension}");
                                }
                            }, () => { Console.WriteLine("Opening file dialog was dismissed"); });
                        }
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }
        
        // This is a direct port of imgui_demo.cpp HelpMarker function
        // https://github.com/ocornut/imgui/blob/master/imgui_demo.cpp#L190
        private void ShowHint(string message)
        {
            // ImGui.TextDisabled("(?)");
            if (ImGui.IsItemHovered())
            {
                // Change background transparency
                ImGui.PushStyleColor(ImGuiCol.PopupBg, new Vector4(0, 0, 0, 0.3f));
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted(message);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
                ImGui.PopStyleColor();
            }
        }
    }
}