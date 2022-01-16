using System.Numerics;
using ImGuiNET;
using SCVE.Editor.ProjectStructure;

namespace SCVE.Editor.ImGuiUi
{
    public class ProjectPanel : IImGuiRenderable
    {
        private AssetPreviewModalPanel _assetPreviewModalPanel;

        public ProjectPanel()
        {
            _assetPreviewModalPanel = new AssetPreviewModalPanel();
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
                
                if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                {
                    _assetPreviewModalPanel.SetOpenedAsset(asset);
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
                if (EditorApp.Instance.OpenedProject is not null)
                {
                    ImGui.Text(EditorApp.Instance.OpenedProject.Name);

                    PushImGuiAssetTreeFolder(EditorApp.Instance.OpenedProject.RootFolder.GetDirectChildFolder("assets"));
                }
                else
                {
                    ImGui.Text("Project is not loaded");
                }

                ImGui.End();
            }
            
            _assetPreviewModalPanel.OnImGuiRender();
        }
    }
}