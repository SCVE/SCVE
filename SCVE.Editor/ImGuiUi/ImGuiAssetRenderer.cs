using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Editing.Visitors;
using SCVE.Editor.Imaging;
using SCVE.Editor.Services;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiAssetRenderer : IAssetVisitor
    {
        private PreviewService _previewService;
        private EditingService _editingService;

        private ProjectPanelService _projectPanelService;

        public ImGuiAssetRenderer(
            PreviewService previewService,
            EditingService editingService,
            ProjectPanelService projectPanelService)
        {
            _previewService = previewService;
            _editingService = editingService;
            _projectPanelService = projectPanelService;
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
                EditorApp.Late("load preview image", () => { _previewService.SetPreviewImage(fileIcon); });
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
                EditorApp.Late("open sequence", () =>
                {
                    _editingService.SetOpenedSequence(asset.Content);
                    _previewService.SwitchSequence(asset.Content);
                });
            }

            if (elementExpanded)
            {
                ImGui.TreePop();
            }
        }

        public void Visit(FolderAsset asset)
        {
            var elementExpanded = ImGui.TreeNodeEx(asset.Name,
                ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                _projectPanelService.ChangeLocation(asset.Location + asset.Name + "/");
            }

            if (elementExpanded)
            {
                ImGui.TreePop();
            }
        }
    }
}