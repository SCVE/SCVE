using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.ImGuiUi
{
    public class AssetPreviewModalPanel : IImGuiRenderable
    {
        private AssetBase _openedAsset;

        private static UnknownTypeAssetPreviewLayout _unknownTypeAssetPreviewLayout = new();

        private AssetPreviewLayout _activePreviewLayout;

        public void SetOpenedAsset(AssetBase asset)
        {
            _openedAsset = asset;

            _unknownTypeAssetPreviewLayout.SetFromAsset(asset);
            _activePreviewLayout = _unknownTypeAssetPreviewLayout;
        }

        public void OnImGuiRender()
        {
            bool previewVisible = true;
            if (_openedAsset is not null)
            {
                ImGui.OpenPopup("Asset Preview");
                if (ImGui.BeginPopupModal("Asset Preview", ref previewVisible))
                {
                    ImGui.TextDisabled($"Previewing asset {_openedAsset.Name}");

                    var typeStr = $"Type: Unknown";
                    var textSize = ImGui.CalcTextSize(typeStr);
                    ImGui.SameLine(ImGui.GetContentRegionAvail().X - textSize.X);
                    ImGui.TextDisabled(typeStr);

                    ImGui.Separator();

                    _activePreviewLayout.OnImGuiRender();

                    if (ImGui.Button("Close"))
                    {
                        ImGui.CloseCurrentPopup();
                        _activePreviewLayout.DisposeResources();
                        _openedAsset = null;
                    }

                    ImGui.EndPopup();
                }
                else
                {
                    ImGui.CloseCurrentPopup();
                    _activePreviewLayout.DisposeResources();
                    _openedAsset = null;
                }
            }
        }
    }
}