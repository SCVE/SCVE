using System;
using System.Numerics;
using ImGuiNET;
using Silk.NET.OpenGL;

namespace SCVE.Editor.ImGuiUi
{
    public class PreviewPanel : IImGuiRenderable
    {
        public void OnImGuiRender()
        {
            if (ImGui.Begin("Preview Panel"))
            {
                var contentRegionAvail = ImGui.GetContentRegionAvail();
                
                var image = EditorApp.Instance.Sampler.PreviewImage;
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

                ImGui.Image((IntPtr)EditorApp.Instance.Sampler.PreviewImage.GpuTexture.GlTexture, imageSize);

                if (ImGui.Button("Re Render Current Frame"))
                {
                    EditorApp.Instance.MarkPreviewDirty();
                }

                ImGui.End();
            }
        }
    }
}