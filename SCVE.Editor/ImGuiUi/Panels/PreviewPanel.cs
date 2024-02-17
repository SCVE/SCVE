using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class PreviewPanel : IImGuiPanel
    {
        public PreviewPanel()
        {
        }

        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Preview Panel"))
            {
                goto END;
            }
            
            var windowPos = ImGui.GetWindowPos();
            var painter = ImGui.GetWindowDrawList();

            // NOTE: ContentRegion is aware of window header
            var contentRegionAvail = ImGui.GetContentRegionAvail();
            var contentRegionMin = ImGui.GetWindowContentRegionMin();

            // var image = _previewService.PreviewImage;
            //
            // var imageSize = Utils.FitRect(contentRegionAvail, new Vector2(image.Width, image.Height));
            //
            // ImGui.SetCursorPos(contentRegionMin + contentRegionAvail / 2 - imageSize / 2);
            //
            // image.ToGpu();
            // ImGui.Image((IntPtr) image.GpuImage.GpuId, imageSize);
            //
            // painter.AddRect(
            //     windowPos + contentRegionMin + contentRegionAvail / 2 - imageSize / 2,
            //     windowPos + contentRegionMin + contentRegionAvail / 2 + imageSize / 2,
            //     0xFFFFFFFF);

            END:
            ImGui.End();
        }
    }
}