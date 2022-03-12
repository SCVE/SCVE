using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Editing.Visitors;
using SCVE.Editor.Imaging;
using SCVE.Editor.Services;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiAssetDrawer : IAssetVisitor
    {
        private PreviewService _previewService;
        private EditingService _editingService;

        public ImGuiAssetDrawer(PreviewService previewService, EditingService editingService)
        {
            _previewService = previewService;
            _editingService = editingService;
        }

        public void Visit(ImageAsset asset)
        {
            var elementExpanded = ImGui.TreeNodeEx(asset.Name,
                ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                var imageSharpTextureLoader = new ImageSharpTextureLoader();

                var fileIconTextureData = imageSharpTextureLoader.Load(asset.Content.RelativePath, false);

                var fileIcon =
                    new ThreeWayImage(
                        new CpuImage(fileIconTextureData.RgbaPixels, fileIconTextureData.Width,
                            fileIconTextureData.Height), "FileIcon");

                fileIcon.ToGpu();
                _previewService.SetPreviewImage(fileIcon);
            }

            if (elementExpanded)
            {
                ImGui.TreePop();
            }
        }

        public void Visit(SequenceAsset asset)
        {
            var elementExpanded = ImGui.TreeNodeEx(asset.Name,
                ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                // _assetPreviewModalPanel.SetOpenedAsset(sequenceAsset);

                _editingService.SetOpenedSequence(asset.Content);
                _previewService.SwitchSequence(asset.Content);
            }

            if (elementExpanded)
            {
                ImGui.TreePop();
            }
        }
    }
}