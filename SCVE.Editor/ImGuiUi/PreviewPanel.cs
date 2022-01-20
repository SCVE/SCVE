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
            if (ImGui.Begin("Preview Panel"))
            {
                var contentRegionAvail = ImGui.GetContentRegionAvail();
                
                var image = _previewModule.PreviewImage;
                if (image is null)
                {
                    ImGui.Text("No preview is available right now");
                    ImGui.End();
                    return;
                }

                var downscaleFactor = 1f;
                if (image.Width > contentRegionAvail.X)
                {
                    if (image.Width > image.Height)
                    {
                        downscaleFactor = contentRegionAvail.X / image.Width;
                    }
                }

                var imageSize = new Vector2(image.Width * downscaleFactor, image.Height * downscaleFactor);

                ImGui.SetCursorPos((contentRegionAvail - imageSize) * 0.5f);

                ImGui.Image((IntPtr)image.GpuTexture.GlTexture, imageSize);

                if (ImGui.Button("Re Render Current Frame"))
                {
                    _previewModule.InvalidateSampledFrame(_editingModule.OpenedSequence.CursorTimeFrame);
                }

                ImGui.End();
            }
        }
    }
}