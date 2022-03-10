using System;
using System.IO;
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

        public MainMenuBar(PreviewService previewService, EditingService editingService,
            ModalManagerService modalManagerService)
        {
            _previewService = previewService;
            _editingService = editingService;
            _modalManagerService = modalManagerService;
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
                    if (ImGui.MenuItem("New", "Ctrl+N"))
                    {
                        _modalManagerService.OpenProjectCreationPanel();
                    }

                    if (ImGui.MenuItem("Open...", "Ctrl+O"))
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

                                _editingService.SetOpenedProject(videoProject);
                                _previewService.SyncVisiblePreview();

                                Console.WriteLine($"Loaded project: {videoProject.Title}");
                            }
                            else
                            {
                                Console.WriteLine($"Unknown file selected: {Path.GetExtension(path)}");
                            }
                        }, () => { Console.WriteLine("Opening file dialog was dismissed"); });
                    }

                    if (ImGui.MenuItem("Save As...", "Ctrl+Shift+S"))
                    {
                        // SaveSceneAs();
                    }

                    if (ImGui.MenuItem("Load test project", "Ctrl+Shift+S"))
                    {
                        var jsonContent = File.ReadAllText("testdata/tester.scveproject");

                        var videoProject = JsonSerializer.Deserialize<VideoProject>(jsonContent,
                            new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        _editingService.SetOpenedProject(videoProject);
                        _previewService.SyncVisiblePreview();
                    }

                    if (_editingService.OpenedProject is not null)
                    {
                        if (ImGui.MenuItem("Save current project", "Ctrl+Shift+S"))
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

                ImGui.EndMenuBar();
            }
        }
    }
}