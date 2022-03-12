using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using SCVE.Editor.Editing.Effects;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.ImGuiUi;
using SCVE.Editor.Services;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Vector2 = System.Numerics.Vector2;

namespace SCVE.Editor
{
    public class EditorApp
    {
        private readonly IWindow _window;
        public GL GL { get; set; }

        public static EditorApp Instance;

        private static bool _dockspaceOpen = true;
        private static bool _optFullscreenPersistant = true;
        private static bool _optFullscreen = _optFullscreenPersistant;

        private static ImGuiDockNodeFlags _dockspaceFlags = ImGuiDockNodeFlags.None;

        public ImFontPtr OpenSansFont;

        private List<IImGuiRenderable> imGuiRenderables;

        private RecentsService _recentsService;

        public EditorApp(IWindow window)
        {
            _window = window;
            Instance = this;
        }

        public void Init()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            foreach (var type in Utils.GetAssignableTypes<IService>())
            {
                serviceCollection.AddSingleton(type);
            }

            serviceCollection.AddSingleton<ClipEvaluator>();
            serviceCollection.AddSingleton<SequenceSampler>();
            serviceCollection.AddSingleton<ImGuiAssetDrawer>();

            serviceCollection.AddSingleton<ProjectPanel>();
            serviceCollection.AddSingleton<SequencePanel>();
            serviceCollection.AddSingleton<PreviewPanel>();
            serviceCollection.AddSingleton<SequenceInfoPanel>();
            serviceCollection.AddSingleton<ClipEffectsPanel>();
            serviceCollection.AddSingleton<MainMenuBar>();
            serviceCollection.AddSingleton<SequenceCreationPanel>();
            serviceCollection.AddSingleton<ProjectCreationPanel>();
            serviceCollection.AddSingleton<FilePickerModalPanel>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            imGuiRenderables = Utils.GetAssignableTypes<IImGuiRenderable>()
                .Select(t => serviceProvider.GetService(t) as IImGuiRenderable)
                .ToList();

            serviceProvider.GetRequiredService<PreviewService>().SyncVisiblePreview();
            _recentsService = serviceProvider.GetRequiredService<RecentsService>();

            _recentsService.TryLoad();
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

            foreach (var imGuiRenderable in imGuiRenderables)
            {
                imGuiRenderable.OnImGuiRender();
            }

            ImGui.ShowDemoWindow();

            ImGui.ShowMetricsWindow();

            ImGui.PopFont();

            ImGui.End();
        }

        public void Exit()
        {
            _recentsService.TrySave();
            _window.IsClosing = true;
        }
    }
}