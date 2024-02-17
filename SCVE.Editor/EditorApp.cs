using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using SCVE.Bootstrap;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Core;
using SCVE.Editor.Services;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Vector2 = System.Numerics.Vector2;

namespace SCVE.Editor
{
    public class EditorApp : BootstrappedApplication
    {
        public Project? ActiveProject { get; private set; }

        public GL GL { get; set; }
        public IInputContext Input { get; set; }

        public static EditorApp Instance;

        private static bool _dockspaceOpen = true;
        private static bool _optFullscreenPersistant = true;
        private static bool _optFullscreen = _optFullscreenPersistant;

        private static ImGuiDockNodeFlags _dockspaceFlags = ImGuiDockNodeFlags.None;

        public ImFontPtr OpenSansFont;

        private List<IImGuiPanel> _imGuiPanels;
        private List<IService> _services;
        private List<IUpdateReceiver> _updateReceivers;
        private List<IKeyPressReceiver> _keyPressReceivers;
        private List<IKeyDownReceiver> _keyDownReceivers;
        private List<IKeyReleaseReceiver> _keyReleaseReceivers;
        private List<IExitReceiver> _exitReceivers;

        private RecentsService _recentsService;
        private SettingsService _settingsService;

        public EditorApp()
        {
            Instance = this;
        }

        public override void Init(GL openGl, ImFontPtr openSansFont)
        {
            base.Init(openGl, openSansFont);
            ImGui.StyleColorsLight();

            IServiceCollection serviceCollection = new ServiceCollection();

            foreach (var type in Utils.GetAssignableTypes<IService>())
            {
                serviceCollection.AddSingleton(type);
            }

            foreach (var type in Utils.GetAssignableTypes<IImGuiPanel>())
            {
                serviceCollection.AddSingleton(type);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _imGuiPanels = Utils.GetAssignableTypes<IImGuiPanel>()
                .Select(t => (serviceProvider.GetService(t) as IImGuiPanel)!)
                .ToList();

            _services = Utils.GetAssignableTypes<IService>()
                .Select(t => (serviceProvider.GetService(t) as IService)!)
                .ToList();

            _updateReceivers = Utils.GetAssignableTypes<IUpdateReceiver>()
                .Select(t => (serviceProvider.GetService(t) as IUpdateReceiver)!)
                .ToList();

            _exitReceivers = Utils.GetAssignableTypes<IExitReceiver>()
                .Select(t => (serviceProvider.GetService(t) as IExitReceiver)!)
                .ToList();

            _keyPressReceivers = Utils.GetAssignableTypes<IKeyPressReceiver>()
                .Select(t => (serviceProvider.GetService(t) as IKeyPressReceiver)!)
                .ToList();

            _keyDownReceivers = Utils.GetAssignableTypes<IKeyDownReceiver>()
                .Select(t => (serviceProvider.GetService(t) as IKeyDownReceiver)!)
                .ToList();

            _keyReleaseReceivers = Utils.GetAssignableTypes<IKeyReleaseReceiver>()
                .Select(t => (serviceProvider.GetService(t) as IKeyReleaseReceiver)!)
                .ToList();

            _settingsService = serviceProvider.GetRequiredService<SettingsService>();
            _settingsService.TryLoad();
        }

        public override void OnImGuiRender()
        {
            ImGui.PushFont(OpenSansFont);

            // We are using the ImGuiWindowFlags_NoDocking flag to make the parent window not dockable into,
            // because it would be confusing to have two docking targets within each others.
            var windowFlags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;
            if (_optFullscreen)
            {
                var viewport = ImGui.GetMainViewport();
                ImGui.SetNextWindowPos(viewport.Pos);
                ImGui.SetNextWindowSize(viewport.Size);
                ImGui.SetNextWindowViewport(viewport.ID);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
                windowFlags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove;
                windowFlags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            }

            // When using ImGuiDockNodeFlags_PassthruCentralNode, DockSpace() will render our background and handle the pass-thru hole, so we ask Begin() to not render a background.
            if ((_dockspaceFlags & ImGuiDockNodeFlags.PassthruCentralNode) != 0)
                windowFlags |= ImGuiWindowFlags.NoBackground;

            // Important: note that we proceed even if Begin() returns false (aka window is collapsed).
            // This is because we want to keep our DockSpace() active. If a DockSpace() is inactive, 
            // all active windows docked into it will lose their parent and become undocked.
            // We cannot preserve the docking relationship between an active window and an inactive docking, otherwise 
            // any change of dockspace/settings would lead to windows being stuck in limbo and never being visible.
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
            ImGui.Begin("DockSpace Demo", ref _dockspaceOpen, windowFlags);
            ImGui.PopStyleVar();

            if (_optFullscreen)
                ImGui.PopStyleVar(2);

            // DockSpace
            var io = ImGui.GetIO();
            var style = ImGui.GetStyle();
            var minWinSizeX = style.WindowMinSize.X;
            style.WindowMinSize.X = 370.0f;
            if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) != 0)
            {
                var dockspaceId = ImGui.GetID("MyDockSpace");
                ImGui.DockSpace(dockspaceId, new Vector2(0.0f, 0.0f), _dockspaceFlags);
            }

            style.WindowMinSize.X = minWinSizeX;

            foreach (var imGuiPanel in _imGuiPanels)
            {
                imGuiPanel.OnImGuiRender();
            }

            ImGui.ShowMetricsWindow();
            ImGui.ShowDemoWindow();

            ImGui.PopFont();

            ImGui.End();
        }

        public override void Exit()
        {
            foreach (var exitReceiver in _exitReceivers)
            {
                exitReceiver.OnExit();
            }
        }

        public override void Update(double delta)
        {
            foreach (var updateReceiver in _updateReceivers)
            {
                updateReceiver.OnUpdate((float) delta);
            }
        }

        public override void OnKeyPressed(Key key)
        {
            foreach (var keyPressReceiver in _keyPressReceivers)
            {
                keyPressReceiver.OnKeyPressed(key);
            }
        }

        public override void OnKeyDown(Key key)
        {
            foreach (var keyDownReceiver in _keyDownReceivers)
            {
                keyDownReceiver.OnKeyDown(key);
            }
        }

        public override void OnKeyReleased(Key key)
        {
            foreach (var keyReleaseReceiver in _keyReleaseReceivers)
            {
                keyReleaseReceiver.OnKeyReleased(key);
            }
        }

        public void OpenProject(Project project)
        {
            ActiveProject = project;
        }
    }
}