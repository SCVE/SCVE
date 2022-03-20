using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ProjectPanel : IImGuiPanel
    {
        private EditingService _editingService;

        private ImGuiAssetRenderer _assetRenderer;

        private ModalManagerService _modalManagerService;

        private ProjectPanelService _projectPanelService;

        public ProjectPanel(
            EditingService editingService,
            ModalManagerService modalManagerService,
            ImGuiAssetRenderer assetRenderer,
            ProjectPanelService projectPanelService)
        {
            _editingService = editingService;
            _modalManagerService = modalManagerService;
            _assetRenderer = assetRenderer;
            _projectPanelService = projectPanelService;
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

        // private void PushImGuiAssetTreeFolder(ProjectAssetFolder folder)
        // {
        //     foreach (var subfolder in folder.Subfolders)
        //     {
        //         var treeExpanded = ImGui.TreeNodeEx(subfolder.InternalName, ImGuiTreeNodeFlags.SpanFullWidth);
        //
        //         if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
        //         {
        //             ShowHint(subfolder.InternalFullPath);
        //         }
        //
        //         if (treeExpanded)
        //         {
        //             PushImGuiAssetTreeFolder(subfolder);
        //             ImGui.TreePop();
        //         }
        //     }
        //
        //     foreach (var asset in folder.Assets)
        //     {
        //         var treeExpanded = ImGui.TreeNodeEx(asset.InternalName, ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);
        //
        //         if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
        //         {
        //             _assetPreviewModalPanel.SetOpenedAsset(asset);
        //         }
        //
        //         if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
        //         {
        //             ShowHint(asset.FileSystemFullPath);
        //         }
        //         else
        //         {
        //             ShowHint(asset.InternalFullPath);
        //         }
        //
        //         if (treeExpanded)
        //         {
        //             ImGui.TreePop();
        //         }
        //     }
        // }

        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Project Panel", _editingService.OpenedProject is null ? ImGuiWindowFlags.None : ImGuiWindowFlags.MenuBar))
            {
                goto END;
            }

            if (_editingService.OpenedProject is null)
            {
                ImGui.Text("Project is not loaded");
                goto END;
            }

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Create"))
                {
                    if (ImGui.MenuItem("Sequence"))
                    {
                        _modalManagerService.OpenSequenceCreationPanel();
                    }
                    
                    if (ImGui.MenuItem("Folder"))
                    {
                        _modalManagerService.OpenFolderCreationPanel();
                    }
                    
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }

            if (!_projectPanelService.HasSelectedLocation)
            {
                ImGui.Text("Location is not selected");
                goto END;
            }

            if (_projectPanelService.CurrentLocation != "/")
            {
                if (ImGui.Button("<-"))
                {
                    _projectPanelService.LevelUp();
                }
            }

            ImGui.Text($"{_editingService.OpenedProject.Title} - {_projectPanelService.CurrentLocation}");

            // PushImGuiAssetTreeFolder(_editingService.OpenedProject.RootFolder.GetDirectChildFolder("assets"));

            PushFolders();

            PushSequences();

            PushImages();

            END:
            ImGui.End();
        }

        private void PushFolders()
        {
            if (ImGui.TreeNodeEx("Folders", ImGuiTreeNodeFlags.SpanFullWidth))
            {
                foreach (var folderAsset in _projectPanelService.Folders)
                {
                    _assetRenderer.Visit(folderAsset);
                }

                ImGui.TreePop();
            }
        }

        private void PushSequences()
        {
            if (ImGui.TreeNodeEx("Sequences", ImGuiTreeNodeFlags.SpanFullWidth))
            {
                foreach (var sequenceAsset in _projectPanelService.Sequences)
                {
                    _assetRenderer.Visit(sequenceAsset);
                }

                ImGui.TreePop();
            }
        }

        private void PushImages()
        {
            if (ImGui.TreeNodeEx("Images", ImGuiTreeNodeFlags.SpanFullWidth))
            {
                foreach (var imageAsset in _projectPanelService.Images)
                {
                    _assetRenderer.Visit(imageAsset);
                }

                ImGui.TreePop();
            }
        }
    }
}