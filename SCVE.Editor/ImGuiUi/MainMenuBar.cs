using ImGuiNET;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{

    public class MainMenuBar : IImGuiRenderable
    {
        private PreviewService _previewService;

        public MainMenuBar(PreviewService previewService)
        {
            _previewService = previewService;
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