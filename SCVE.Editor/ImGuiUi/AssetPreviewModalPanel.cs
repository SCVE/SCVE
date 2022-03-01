using ImGuiNET;
using SCVE.Editor.ProjectStructure;

namespace SCVE.Editor.ImGuiUi
{
    public class AssetPreviewModalPanel : IImGuiRenderable
    {
        private ProjectAsset<> _openedAsset;

        private static TextAssetPreviewLayout _textAssetPreviewLayout = new();
        private static ImageAssetPreviewLayout _imageAssetPreviewLayout = new();
        private static UnknownTypeAssetPreviewLayout _unknownTypeAssetPreviewLayout = new();
        private static NotFoundAssetPreviewLayout _notFoundAssetPreviewLayout = new();
        
        private AssetPreviewLayout _activePreviewLayout;

        public void SetOpenedAsset(ProjectAsset<> asset)
        {
            _openedAsset = asset;

            if (!asset.ExistsInFileSystem)
            {
                _notFoundAssetPreviewLayout.SetFromAsset(asset);
                _activePreviewLayout = _notFoundAssetPreviewLayout;
            }
            else if (_openedAsset.Type == "TEXT")
            {
                _textAssetPreviewLayout.SetFromAsset(asset);
                _activePreviewLayout = _textAssetPreviewLayout;
            }
            else if (_openedAsset.Type == "IMAGE")
            {
                _imageAssetPreviewLayout.SetFromAsset(asset);
                _activePreviewLayout = _imageAssetPreviewLayout;
            }
            else
            {
                _unknownTypeAssetPreviewLayout.SetFromAsset(asset);
                _activePreviewLayout = _unknownTypeAssetPreviewLayout;
            }
        }

        public void OnImGuiRender()
        {
            bool previewVisible = true;
            if (_openedAsset is not null)
            {
                ImGui.OpenPopup("Asset Preview");
                if (ImGui.BeginPopupModal("Asset Preview", ref previewVisible))
                {
                    ImGui.TextDisabled($"Previewing asset {_openedAsset.InternalName}");

                    var typeStr  = $"Type: {_openedAsset.Type}";
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