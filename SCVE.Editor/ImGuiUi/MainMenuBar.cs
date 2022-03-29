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
using SCVE.Editor.ImGuiUi.Panels;
using SCVE.Editor.Late;
using SCVE.Editor.Services;
using SCVE.Editor.Services.Loaders;

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

        private ProjectLoaderService _projectLoaderService;
        private ImageLoaderService _imageLoaderService;

        private ClipEvaluator _evaluator;

        private FilePickerModalPanel _openProjectFilePickerModalPanel;
        private FilePickerModalPanel _addImageFilePickerModalPanel;

        public MainMenuBar(
            PreviewService previewService,
            EditingService editingService,
            ModalManagerService modalManagerService,
            RecentsService recentsService,
            ProjectPanelService projectPanelService,
            BackgroundJobRunner backgroundJobRunner,
            ClipEvaluator evaluator,
            ProjectLoaderService projectLoaderService,
            ImageLoaderService imageLoaderService)
        {
            _previewService = previewService;
            _editingService = editingService;
            _modalManagerService = modalManagerService;
            _recentsService = recentsService;
            _projectPanelService = projectPanelService;
            _backgroundJobRunner = backgroundJobRunner;
            _evaluator = evaluator;
            _projectLoaderService = projectLoaderService;
            _imageLoaderService = imageLoaderService;
            _openProjectFilePickerModalPanel = new FilePickerModalPanel();
            _addImageFilePickerModalPanel = new FilePickerModalPanel();
        }

        public void OnImGuiRender()
        {
            if (ImGui.BeginMenuBar())
            {
                bool shouldOpenOpenProjectFilePicker = false;
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New Project", "Ctrl+N"))
                    {
                        _modalManagerService.OpenProjectCreationPanel();
                    }

                    // NOTE: We can't call ImGui.OpenPopup inside MenuItem, because of the ID stack.

                    if (ImGui.MenuItem("Open Project", "Ctrl+O"))
                    {
                        shouldOpenOpenProjectFilePicker = true;
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
                                    
                                    EditorApp.Late(new OpenProjectLateTask(videoProject, recent));
                                    
                                    // NOTE: break is needed, because _recentsService.NoticeOpen() modifies original list
                                    // break;
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
                        }
                    }

                    if (ImGui.MenuItem("Exit"))
                    {
                        EditorApp.Instance.Exit();
                    }

                    ImGui.EndMenu();
                }

                HandleOpenProjectFilePicker(shouldOpenOpenProjectFilePicker);

                if (_editingService.OpenedSequence is not null)
                {
                    if (ImGui.BeginMenu("Sequence"))
                    {
                        if (ImGui.MenuItem("Add Track"))
                        {
                            _editingService.OpenedSequence.Tracks.Add(Track.CreateNew());
                        }

                        ImGui.EndMenu();
                    }
                }

                if (_editingService.OpenedProject is not null)
                {
                    bool shouldOpenAddImageFilePicker = false;
                    if (ImGui.BeginMenu("Assets"))
                    {
                        if (ImGui.MenuItem("Add Image"))
                        {
                            shouldOpenAddImageFilePicker = true;
                        }

                        ImGui.EndMenu();
                    }

                    HandleAddImageFilePicker(shouldOpenAddImageFilePicker);
                }


                if (ImGui.BeginMenu("Windows"))
                {
                    if (ImGui.MenuItem("Settings"))
                    {
                        _modalManagerService.OpenSettingsPanel();
                    }

                    ImGui.EndMenu();
                }

                ImGui.EndMenuBar();
            }
        }

        private void HandleAddImageFilePicker(bool shouldOpenAddImageFilePicker)
        {
            if (shouldOpenAddImageFilePicker)
            {
                Console.WriteLine("Opening AddImage FilePicker");
                _addImageFilePickerModalPanel.Open(Environment.CurrentDirectory, "Add Image");
            }

            string path = "";
            if (_addImageFilePickerModalPanel.OnImGuiRender(ref path))
            {
                if (_imageLoaderService.TryLoad(path, out var image))
                {
                    var fileName = Path.GetFileName(path);
                    var imageAsset = ImageAsset.CreateNew(
                        name: fileName,
                        location: _projectPanelService.CurrentLocation,
                        content: image
                    );

                    EditorApp.Late(new AddImageLateTask(imageAsset));
                    // EditorApp.Late("add image", () => { _editingService.OpenedProject.AddImage(imageAsset); });
                }
            }
        }

        private void HandleOpenProjectFilePicker(bool shouldOpenOpenProjectFilePicker)
        {
            if (shouldOpenOpenProjectFilePicker)
            {
                Console.WriteLine("Opening OpenProject FilePicker");
                _openProjectFilePickerModalPanel.Open(Environment.CurrentDirectory, "Open Project");
            }

            string path = "";
            if (_openProjectFilePickerModalPanel.OnImGuiRender(ref path))
            {
                if (_projectLoaderService.TryLoad(path, out var videoProject))
                {
                    if (_editingService.OpenedProject is not null)
                    {
                        Console.WriteLine("Loading new project, while there is a loaded one");
                    }

                    EditorApp.Late(new OpenProjectLateTask(videoProject, path));
                }
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