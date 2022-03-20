using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Effects;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.ImGuiUi;
using SCVE.Editor.ImGuiUi.Panels;
using SCVE.Editor.ImGuiUi.Services;
using SCVE.Editor.Services;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Vector2 = System.Numerics.Vector2;

namespace SCVE.Editor
{
    public class EditorApp
    {
        private readonly IWindow _window;
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
        private List<IExitReceiver> _exitReceivers;
        private List<IFileDropReceiver> _fileDropReceivers;

        private RecentsService _recentsService;
        private SettingsService _settingsService;

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
            foreach (var type in Utils.GetAssignableTypes<IImGuiPanel>())
            {
                serviceCollection.AddSingleton(type);
            }

            serviceCollection.AddSingleton<ClipEvaluator>();
            serviceCollection.AddSingleton<SequenceSampler>();
            serviceCollection.AddSingleton<ImGuiAssetRenderer>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            _imGuiPanels = Utils.GetAssignableTypes<IImGuiPanel>()
                .Select(t => serviceProvider.GetService(t) as IImGuiPanel)
                .ToList();

            _services = Utils.GetAssignableTypes<IService>()
                .Select(t => serviceProvider.GetService(t) as IService)
                .ToList();

            _updateReceivers = Utils.GetAssignableTypes<IUpdateReceiver>()
                .Select(t => serviceProvider.GetService(t) as IUpdateReceiver)
                .ToList();

            _exitReceivers = Utils.GetAssignableTypes<IExitReceiver>()
                .Select(t => serviceProvider.GetService(t) as IExitReceiver)
                .ToList();

            _keyPressReceivers = Utils.GetAssignableTypes<IKeyPressReceiver>()
                .Select(t => serviceProvider.GetService(t) as IKeyPressReceiver)
                .ToList();

            _fileDropReceivers = Utils.GetAssignableTypes<IFileDropReceiver>()
                .Select(t => serviceProvider.GetService(t) as IFileDropReceiver)
                .ToList();

            serviceProvider.GetRequiredService<PreviewService>().SyncVisiblePreview();

            _recentsService = serviceProvider.GetRequiredService<RecentsService>();
            _recentsService.TryLoad();

            _settingsService = serviceProvider.GetRequiredService<SettingsService>();
            _settingsService.TryLoad();
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

            foreach (var imGuiPanel in _imGuiPanels)
            {
                imGuiPanel.OnImGuiRender();
            }

            ImGui.ShowMetricsWindow();

            ImGui.PopFont();

            ImGui.End();
        }

        public void Exit()
        {
            foreach (var exitReceiver in _exitReceivers)
            {
                exitReceiver.OnExit();
            }

            _recentsService.TrySave();
            // TODO: Fix error with exiting
            _window.IsClosing = true;
        }

        public void Update(double delta)
        {
            foreach (var updateReceiver in _updateReceivers)
            {
                updateReceiver.OnUpdate((float) delta);
            }
        }

        public void OnKeyPressed(Key key)
        {
            foreach (var keyPressReceiver in _keyPressReceivers)
            {
                keyPressReceiver.OnKeyPressed(key);
            }
        }

        /// <summary>
        /// Validates the passed paths whether they can be parsed and triggers event OnFileDrop
        /// on each service implementing the interface IFileDropReceiver.
        /// </summary>
        /// <param name="paths">to the dropping files.</param>
        public void OnFileDrop(string[] paths)
        {
            if (IsValidDropFiles(paths))
            {
                foreach (var fileDropReceiver in _fileDropReceivers)
                {
                    fileDropReceiver.OnFileDrop(paths);
                }

                return;
            }

            Console.WriteLine("Invalid combination of paths:");
            foreach (var path in paths)
            {
                Console.WriteLine(path);
            }
        }

        /// <summary>
        /// Checks if the passed paths combination can be parsed.
        /// </summary>
        /// <param name="paths">All paths to the dropped files.</param>
        /// <returns></returns>
        private bool IsValidDropFiles(string[] paths)
        {
            if (paths.Length == 0) return false;


            // var extends = new List<string>(paths.Length);
            bool foundScve = false;
            bool foundImage = false;
            foreach (var path in paths)
            {
                var extension = Path.GetExtension(path).ToLower();
                switch (extension)
                {
                    case ".scveproject":
                    {
                        // .scveproject + .scveproject = invalid
                        if (foundScve)
                        {
                            return false;
                        }

                        // .scveproject + image = invalid
                        if (foundImage)
                        {
                            return false;
                        }

                        foundScve = true;

                        break;
                    }
                    case ".png":
                    case ".jpeg":
                    {
                        // .scveproject + image = invalid
                        if (foundScve)
                        {
                            return false;
                        }

                        foundImage = true;

                        break;
                    }
                    default:
                        return false;
                }
            }

            return true;
        }
    }
}