using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Modules;

namespace SCVE.Editor.ImGuiUi
{
    public class PreviewPanel : IImGuiRenderable
    {
        private readonly EditingModule _editingModule;
        private readonly PreviewModule _previewModule;

        public PreviewPanel()
        {
            _previewModule = EditorApp.Modules.Get<PreviewModule>();
            _editingModule = EditorApp.Modules.Get<EditingModule>();
        }

        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Preview Panel"))
            {
                goto END;
            }

            var contentRegionAvail = ImGui.GetContentRegionAvail();
            var windowPos = ImGui.GetWindowPos();
            var windowSize = ImGui.GetWindowSize();
            var painter = ImGui.GetWindowDrawList();

            var image = _previewModule.PreviewImage;

            var downscaleFactor = 1f;
            if (image.Width > contentRegionAvail.X)
            {
                if (image.Width > image.Height)
                {
                    downscaleFactor = contentRegionAvail.X / image.Width;
                }
            }

            var imageSize = new Vector2(image.Width * downscaleFactor, image.Height * downscaleFactor);

            ImGui.SetCursorPos((windowSize - contentRegionAvail) / 2 + (contentRegionAvail - imageSize) / 2);
            // ImGui.SetCursorPos(windowSize / 2 + imageSize / 2);

            image.ToGpu();
            ImGui.Image((IntPtr) image.GpuImage.GpuId, imageSize);

            painter.AddRect(
                new Vector2(
                    windowPos.X + contentRegionAvail.X / 2 + (windowSize.X - contentRegionAvail.X) / 2 -
                    imageSize.X / 2,
                    windowPos.Y + contentRegionAvail.Y / 2 + (windowSize.X - contentRegionAvail.X) / 2 -
                    imageSize.Y / 2
                ),
                new Vector2(
                    windowPos.X + contentRegionAvail.X / 2 + imageSize.X / 2,
                    windowPos.Y + contentRegionAvail.Y / 2 + imageSize.Y / 2
                ), 0xFFFFFFFF);

            END:
            ImGui.End();
        }
    }
}