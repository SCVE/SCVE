using System;
using System.Numerics;
using ImGuiNET;
using OpenTK.Mathematics;
using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Lifecycle;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Rendering;
using Vector2 = System.Numerics.Vector2;

namespace SCVE.Editor
{
    public class EditorApp : IEngineRunnable
    {
        ImGuiController _controller;

        public void Init()
        {
            ScveEngine.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
            ScveEngine.Instance.Input.Scroll            += InputOnScroll;
            ScveEngine.Instance.Input.CharTyped         += InputOnCharTyped;

            _controller = new ImGuiController(ScveEngine.Instance.MainWindow.Width, ScveEngine.Instance.MainWindow.Height);
        }

        private void InputOnCharTyped(char obj)
        {
            _controller.PressChar(obj);
        }

        private void InputOnScroll(float x, float y)
        {
            _controller.MouseScroll(x, y);
        }

        private void InputOnWindowSizeChanged(int width, int height)
        {
            // Tell ImGui of the new size
            _controller.WindowResized(width, height);
        }

        private static bool dockspaceOpen = true;
        private static bool _optFullscreenPersistant = true;
        private static bool _optFullscreen = _optFullscreenPersistant;

        private static ImGuiDockNodeFlags _dockspaceFlags = ImGuiDockNodeFlags.None;

        public void Render(IRenderer renderer)
        {
            ImGui.PushFont(_controller.arialFont);

            // We are using the ImGuiWindowFlags_NoDocking flag to make the parent window not dockable into,
            // because it would be confusing to have two docking targets within each others.
            ImGuiWindowFlags window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;
            if (_optFullscreen)
            {
                ImGuiViewportPtr viewport = ImGui.GetMainViewport();
                ImGui.SetNextWindowPos(viewport.Pos);
                ImGui.SetNextWindowSize(viewport.Size);
                ImGui.SetNextWindowViewport(viewport.ID);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
                window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
                window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            }

            // When using ImGuiDockNodeFlags_PassthruCentralNode, DockSpace() will render our background and handle the pass-thru hole, so we ask Begin() to not render a background.
            if ((_dockspaceFlags & ImGuiDockNodeFlags.PassthruCentralNode) != 0)
                window_flags |= ImGuiWindowFlags.NoBackground;

            // Important: note that we proceed even if Begin() returns false (aka window is collapsed).
            // This is because we want to keep our DockSpace() active. If a DockSpace() is inactive, 
            // all active windows docked into it will lose their parent and become undocked.
            // We cannot preserve the docking relationship between an active window and an inactive docking, otherwise 
            // any change of dockspace/settings would lead to windows being stuck in limbo and never being visible.
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
            ImGui.Begin("DockSpace Demo", ref dockspaceOpen, window_flags);
            ImGui.PopStyleVar();

            if (_optFullscreen)
                ImGui.PopStyleVar(2);

            // DockSpace
            ImGuiIOPtr    io          = ImGui.GetIO();
            ImGuiStylePtr style       = ImGui.GetStyle();
            float         minWinSizeX = style.WindowMinSize.X;
            style.WindowMinSize.X = 370.0f;
            if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) != 0)
            {
                uint dockspace_id = ImGui.GetID("MyDockSpace");
                ImGui.DockSpace(dockspace_id, new Vector2(0.0f, 0.0f), _dockspaceFlags);
            }

            style.WindowMinSize.X = minWinSizeX;

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
                        ScveEngine.Instance.RequestTerminate();
                    }
                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
            
            // TODO: Render separate panels
            
            

            ImGui.ShowDemoWindow();
            ImGui.ShowMetricsWindow();
            _controller.Render();
        }

        public void Update(float deltaTime)
        {
            _controller.Update(deltaTime);
        }
    }
}