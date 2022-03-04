﻿using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class ProjectPanel : IImGuiRenderable
    {
        private EditingService _editingService;

        private AssetPreviewModalPanel _assetPreviewModalPanel;

        private SequenceCreationPanel _sequenceCreationPanel;

        public ProjectPanel(EditingService editingService, SequenceCreationPanel sequenceCreationPanel)
        {
            _assetPreviewModalPanel = new AssetPreviewModalPanel();
            _editingService = editingService;
            _sequenceCreationPanel = sequenceCreationPanel;
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

                if (ImGui.Button("Create new sequence"))
                {
                    _sequenceCreationPanel.Visible = true;
                }
            }
            else
            {
                ImGui.Text("Project is not loaded");
            }

            END:
            ImGui.End();

            _assetPreviewModalPanel.OnImGuiRender();
        }
    }
}