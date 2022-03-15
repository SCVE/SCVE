using System;
using System.Collections.Concurrent;
using System.IO;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Background;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class MainMenuBar : IImGuiPanel
    {
        private PreviewService _previewService;
        private EditingService _editingService;

        private ModalManagerService _modalManagerService;

        private RecentsService _recentsService;

        private ProjectPanelService _projectPanelService;

        private BackgroundJobRunner _backgroundJobRunner;

        private ClipEvaluator _evaluator;

        public MainMenuBar(PreviewService previewService,
            EditingService editingService,
            ModalManagerService modalManagerService,
            RecentsService recentsService,
            ProjectPanelService projectPanelService,
            BackgroundJobRunner backgroundJobRunner,
            ClipEvaluator evaluator)
        {
            _previewService = previewService;
            _editingService = editingService;
            _modalManagerService = modalManagerService;
            _recentsService = recentsService;
            _projectPanelService = projectPanelService;
            _backgroundJobRunner = backgroundJobRunner;
            _evaluator = evaluator;
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
                                var videoProject = Utils.ReadJson<VideoProject>(path);

                                _editingService.SetOpenedProject(videoProject, path);
                                _recentsService.NoticeOpen(path);
                                _projectPanelService.ChangeLocation("/");
                                _previewService.SyncVisiblePreview();

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

                                if (ImGui.MenuItem(fileName, File.Exists(recent)))
                                {
                                    var videoProject = Utils.ReadJson<VideoProject>(recent);

                                    _editingService.SetOpenedProject(videoProject, recent);
                                    _recentsService.NoticeOpen(recent);
                                    _projectPanelService.ChangeLocation("/");
                                    _previewService.SyncVisiblePreview();

                                    Console.WriteLine($"Loaded Recent Project: {videoProject.Title}");

                                    // NOTE: break is needed, because _recentsService.NoticeOpen() modifies original list
                                    break;
                                }

                                ShowHint(recent);
                            }

                            ImGui.EndMenu();
                        }
                    }

                    if (_editingService.OpenedProject is not null)
                    {
                        if (ImGui.MenuItem("Save Project", "Ctrl+S"))
                        {
                            if (_editingService.OpenedProjectPath is not null)
                            {
                                Utils.WriteJson(_editingService.OpenedProject, _editingService.OpenedProjectPath);
                            }
                            else
                            {
                                // TODO
                                throw new NotImplementedException("Saving of just-created projects is not currently supported");
                            }
                        }
                    }

                    if (ImGui.MenuItem("Exit"))
                    {
                        EditorApp.Instance.Exit();
                    }

                    ImGui.EndMenu();
                }

                if (_editingService.OpenedSequence is not null)
                {
                    if (ImGui.BeginMenu("Sequence"))
                    {
                        if (ImGui.MenuItem("Render start to end", "Ctrl+R"))
                        {
                            for (var i = 0; i < _editingService.OpenedSequence.FrameLength; i++)
                            {
                                int localI = i;
                                var job = new RenderFrameBackgroundJob(
                                    sequence: _editingService.OpenedSequence,
                                    sampler: new SequenceSampler(_evaluator),
                                    resolution: new ScveVector2i(1280, 720),
                                    frame: i,
                                    onFinished: (image) =>
                                    {
                                        Console.WriteLine($"Finished rendering frame {localI} in background");
                                        _previewService.SetRenderedFrame(localI, image);
                                    }
                                );
                                _backgroundJobRunner.PushJob(job);
                            }
                        }

                        if (ImGui.MenuItem("Add Track"))
                        {
                            _editingService.OpenedSequence.Tracks.Add(Track.CreateNew());
                        }

                        ImGui.EndMenu();
                    }
                }

                if (ImGui.BeginMenu("Assets"))
                {
                    if (_editingService.OpenedProject is not null)
                    {
                        if (ImGui.MenuItem("Add Image"))
                        {
                            _modalManagerService.OpenFilePickerPanel(Environment.CurrentDirectory, () =>
                            {
                                string path = _modalManagerService.FilePickerSelectedPath;

                                var extension = Path.GetExtension(path);
                                if (extension is ".jpeg" or ".png")
                                {
                                    var fileName = Path.GetFileName(path);
                                    var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, path);
                                    var imageAsset = new ImageAsset()
                                    {
                                        Guid = Guid.NewGuid(),
                                        Name = fileName,
                                        Location = _projectPanelService.CurrentLocation,
                                        Content = new Image()
                                        {
                                            Guid = Guid.NewGuid(),
                                            RelativePath = relativePath
                                        }
                                    };
                                    _editingService.OpenedProject.AddImage(imageAsset);
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

                if (ImGui.BeginMenu("Settings"))
                {
                    _modalManagerService.OpenSettingsPanel();
                    
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