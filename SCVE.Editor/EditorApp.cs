using ImGuiNET;
using SCVE.Editor.Editing;
using SCVE.Editor.Effects;
using SCVE.Editor.ImGuiUi;
using SCVE.Editor.Modules;
using SCVE.Editor.ProjectStructure;
using Silk.NET.OpenGL;
using Vector2 = System.Numerics.Vector2;

namespace SCVE.Editor
{
    public class EditorApp
    {
        public GL GL { get; set; }

        public static EditorApp Instance;

        private ModulesContainer _modules;
        public static ModulesContainer Modules => Instance._modules;

        private static bool _dockspaceOpen = true;
        private static bool _optFullscreenPersistant = true;
        private static bool _optFullscreen = _optFullscreenPersistant;

        private static ImGuiDockNodeFlags _dockspaceFlags = ImGuiDockNodeFlags.None;

        private ProjectPanel _projectPanel;
        private SequencePanel _sequencePanel;
        private PreviewPanel _previewPanel;
        private SequenceInfoPanel _sequenceInfoPanel;
        private ClipEffectsPanel _clipEffectsPanel;

        public ImFontPtr OpenSansFont;
        private MainMenuBar _mainMenuBar;

        public EditorApp()
        {
            Instance = this;
        }

        public void Init()
        {
            _modules = new ModulesContainer();
            _modules.Add<AssetCacheModule>();
            _modules.Add<SamplerModule>();
            _modules.Add<EditingModule>();
            _modules.Add<PreviewModule>();
            _modules.CrossReference();

            // NOTE: modules are inited in order of declaration above
            _modules.Init();

            _projectPanel = new();
            _sequencePanel = new();
            _previewPanel = new();
            _sequenceInfoPanel = new();
            _clipEffectsPanel = new();
            _mainMenuBar = new();

            _modules.Get<PreviewModule>().SyncVisiblePreview();
        }

        public void OnImGuiRender()
        {
            ImGui.PushFont(OpenSansFont);

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
                window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove;
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
            ImGui.Begin("DockSpace Demo", ref _dockspaceOpen, window_flags);
            ImGui.PopStyleVar();

            if (_optFullscreen)
                ImGui.PopStyleVar(2);

            // DockSpace
            ImGuiIOPtr io = ImGui.GetIO();
            ImGuiStylePtr style = ImGui.GetStyle();
            float minWinSizeX = style.WindowMinSize.X;
            style.WindowMinSize.X = 370.0f;
            if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) != 0)
            {
                uint dockspace_id = ImGui.GetID("MyDockSpace");
                ImGui.DockSpace(dockspace_id, new Vector2(0.0f, 0.0f), _dockspaceFlags);
            }

            style.WindowMinSize.X = minWinSizeX;

            _mainMenuBar.OnImGuiRender();

            ImGui.ShowDemoWindow();

            Modules.Update();

            _projectPanel.OnImGuiRender();
            _sequencePanel.OnImGuiRender();
            _previewPanel.OnImGuiRender();
            _sequenceInfoPanel.OnImGuiRender();
            _clipEffectsPanel.OnImGuiRender();

            ImGui.ShowMetricsWindow();
        }
    }
}