using System.IO;
using System.Text.Json;
using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{

    public class MainMenuBar : IImGuiRenderable
    {
        private PreviewService _previewService;
        private EditingService _editingService;

        public MainMenuBar(PreviewService previewService, EditingService editingService)
        {
            _previewService = previewService;
            _editingService = editingService;
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
                        // NewScene();
                    }

                    if (ImGui.MenuItem("Open...", "Ctrl+O"))
                    {
                        // OpenScene();
                    }

                    if (ImGui.MenuItem("Save As...", "Ctrl+Shift+S"))
                    {
                        // SaveSceneAs();
                    }

                    if (ImGui.MenuItem("Load test project", "Ctrl+Shift+S"))
                    {
                        var jsonContent = File.ReadAllText("testdata/tester.json");

                        var videoProject = JsonSerializer.Deserialize<VideoProject>(jsonContent, new JsonSerializerOptions()
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
                            var jsonContent = JsonSerializer.Serialize(_editingService.OpenedProject, new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true,
                                WriteIndented = true
                            });

                            File.WriteAllText("testdata/savetest.json", jsonContent);
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

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }
    }
}