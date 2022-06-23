using System;
using ImGuiNET;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Editing.Visitors;
using SCVE.Editor.Imaging;
using SCVE.Editor.Late;
using SCVE.Editor.Services;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiAssetRenderer : IAssetVisitor
    {
        private PreviewService _previewService;
        private EditingService _editingService;

        private ProjectPanelService _projectPanelService;

        private DragDropAssetToSequenceService _dragDropAssetToSequenceService;

        public ImGuiAssetRenderer(
            PreviewService previewService,
            EditingService editingService,
            ProjectPanelService projectPanelService,
            DragDropAssetToSequenceService dragDropAssetToSequenceService)
        {
            _previewService = previewService;
            _editingService = editingService;
            _projectPanelService = projectPanelService;
            _dragDropAssetToSequenceService = dragDropAssetToSequenceService;
        }

        public void Visit(ImageAsset asset)
        {
            var elementExpanded = ImGui.TreeNodeEx(asset.Name,
                ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.SpanFullWidth);

            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                var imageSharpTextureLoader = new ImageSharpTextureLoader();

                var assetTextureData = imageSharpTextureLoader.Load(asset.Content.RelativePath, false);

                var textureImage =
                    new ThreeWayImage(
                        new CpuImage(assetTextureData.RgbaPixels, assetTextureData.Width,
                            assetTextureData.Height), "AssetTextureImage");

                textureImage.ToGpu();


                EditorApp.Late(new LoadPreviewImageLateTask(textureImage));
            }

            if (ImGui.IsItemActivated())
            {
                _dragDropAssetToSequenceService.SetDraggedAsset(asset);
                Console.WriteLine("Started Drag");
            }

            if (ImGui.IsItemDeactivated() && _dragDropAssetToSequenceService.Frame != -1 && _dragDropAssetToSequenceService.Track != -1)
            {
                EditorApp.Late(new CreateImageAssetClipLateTask(asset, _dragDropAssetToSequenceService.Frame, _dragDropAssetToSequenceService.Track));

                _dragDropAssetToSequenceService.Reset();
                Console.WriteLine("Ended Drag");
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

            if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
            {
                // _assetPreviewModalPanel.SetOpenedAsset(sequenceAsset);
                EditorApp.Late(new OpenSequenceLateTask(asset.Content));
            }

            // if (ImGui.IsItemActivated())
            // {
            //     _dragDropAssetToSequenceService.SetDraggedAsset(asset);
            //     Console.WriteLine("Started Drag");
            // }
            //
            // if (ImGui.IsItemDeactivated())
            // {
            //     EditorApp.Late(new CreateSequenceAssetClipLateTask(asset, _dragDropAssetToSequenceService.Frame, _dragDropAssetToSequenceService.Track));
            //     
            //     _dragDropAssetToSequenceService.SetDraggedAsset(null);
            //     Console.WriteLine("Ended Drag");
            // }

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

            // if (ImGui.IsItemActivated())
            // {
            //     _dragDropAssetToSequenceService.SetDraggedAsset(asset);
            //     Console.WriteLine("Started Drag");
            // }
            //
            // if (ImGui.IsItemDeactivated())
            // {
            //     _dragDropAssetToSequenceService.SetDraggedAsset(null);
            //     Console.WriteLine("Ended Drag");
            // }

            if (elementExpanded)
            {
                ImGui.TreePop();
            }
        }
    }
}