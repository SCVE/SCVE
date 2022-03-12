using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class ProjectPanel : IImGuiRenderable
    {
        private EditingService _editingService;

        private ImGuiAssetDrawer _assetDrawer;

        private ModalManagerService _modalManagerService;

        public ProjectPanel(
            EditingService editingService, 
            ModalManagerService modalManagerService, 
            ImGuiAssetDrawer assetDrawer)
        {
            _editingService = editingService;
            _modalManagerService = modalManagerService;
            _assetDrawer = assetDrawer;
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
            if (!ImGui.Begin("Project Panel"))
            {
                goto END;
            }

            if (_editingService.OpenedProject is not null)
            {
                ImGui.Text(_editingService.OpenedProject.Title);

                // PushImGuiAssetTreeFolder(_editingService.OpenedProject.RootFolder.GetDirectChildFolder("assets"));

                PushSequences();

                if (ImGui.Button("Create new sequence"))
                {
                    _modalManagerService.OpenSequenceCreationPanel();
                }

                PushImages();
            }
            else
            {
                ImGui.Text("Project is not loaded");
            }

            END:
            ImGui.End();
        }

        private void PushSequences()
        {
            var treeRootExpanded =
                ImGui.TreeNodeEx("Sequences", ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

            foreach (var sequenceAsset in _editingService.OpenedProject.Sequences)
            {
                _assetDrawer.Visit(sequenceAsset);
            }

            if (treeRootExpanded)
            {
                ImGui.TreePop();
            }
        }

        private void PushImages()
        {
            var treeRootExpanded =
                ImGui.TreeNodeEx("Images", ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

            foreach (var imageAsset in _editingService.OpenedProject.Images)
            {
                _assetDrawer.Visit(imageAsset);
            }

            if (treeRootExpanded)
            {
                ImGui.TreePop();
            }
        }
    }
}