using System;
using System.IO;
using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor
{
    public class ProjectPanel
    {
        private Project _project;

        private ProjectAsset _openedAsset;
        private string _openedAssetPreviewContent;

        public void LoadProject(string path)
        {
            if (Project.PathIsProject(path))
            {
                _project = Project.LoadFrom(path);
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

        private void PushImGuiAssetTreeFolder(ProjectAssetFolder folder)
        {
            foreach (var subfolder in folder.Subfolders)
            {
                var treeExpanded = ImGui.TreeNodeEx(subfolder.InternalName, ImGuiTreeNodeFlags.SpanFullWidth);

                if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
                {
                    ShowHint(subfolder.FileSystemFullPath);
                }
                else
                {
                    ShowHint(subfolder.InternalFullPath);
                }

                if (treeExpanded)
                {
                    PushImGuiAssetTreeFolder(subfolder);
                    ImGui.TreePop();
                }
            }

            foreach (var asset in folder.Assets)
            {
                var treeExpanded = ImGui.TreeNodeEx(asset.InternalName, ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

                if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                {
                    _openedAsset = asset;
                    if (_openedAsset.Type == "TEXT")
                    {
                        _openedAssetPreviewContent = File.ReadAllText(_openedAsset.FileSystemFullPath);
                    }
                    else
                    {
                        _openedAssetPreviewContent = $"Asset of type {_openedAsset.Type} is not currently supported for preview" +
                                                     $"{_openedAsset.FileSystemFullPath}";
                    }
                }

                if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
                {
                    ShowHint(asset.FileSystemFullPath);
                }
                else
                {
                    ShowHint(asset.InternalFullPath);
                }

                if (treeExpanded)
                {
                    ImGui.TreePop();
                }
            }
        }

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Project Panel"))
            {
                if (_project is not null)
                {
                    ImGui.Text(_project.Name);

                    PushImGuiAssetTreeFolder(_project.RootFolder.GetDirectChildFolder("assets"));
                }
                else
                {
                    ImGui.Text("Project is not loaded");
                }

                ImGui.End();
            }

            bool previewVisible = true;
            if (_openedAsset is not null)
            {
                ImGui.OpenPopup("Asset Preview");
                if (ImGui.BeginPopupModal("Asset Preview", ref previewVisible))
                {
                    ImGui.TextDisabled($"Previewing asset {_openedAsset.InternalName}");

                    ImGui.TextUnformatted(_openedAssetPreviewContent);

                    if (ImGui.Button("Close"))
                    {
                        _openedAsset = null;
                        ImGui.CloseCurrentPopup();
                    }

                    ImGui.EndPopup();
                }
            }
        }
    }
}