using System;
using System.Numerics;
using ImGuiNET;
using SCVE.Engine.ImageSharpBindings;

namespace SCVE.Editor.ImGuiUi
{
    public class PreviewPanel : IImGuiRenderable
    {
        private Texture _activeTexture;

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Preview Panel"))
            {
                var contentRegionAvail = ImGui.GetContentRegionAvail();

                var image           = EditorApp.Instance.Sampler.PreviewImage;
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

                if (_activeTexture is null)
                {
                    UploadNewTexture();
                }

                ImGui.Image((IntPtr)_activeTexture.GlTexture, imageSize);

                if (ImGui.Button("Render Current Frame"))
                {
                    EditorApp.Instance.Sampler.Sample(EditorApp.Instance.OpenedSequence, EditorApp.Instance.OpenedSequence.CursorTimeFrame);
                    UploadNewTexture();
                }

                ImGui.End();
            }
        }

        private void UploadNewTexture()
        {
            if (_activeTexture is not null)
            {
                _activeTexture.Dispose();
            }

            var bytes = ImageSharpTextureLoader.ImageToBytes(EditorApp.Instance.Sampler.PreviewImage);

            _activeTexture = new Texture(EditorApp.Instance.GL, EditorApp.Instance.Sampler.PreviewImage.Width, EditorApp.Instance.Sampler.PreviewImage.Height, bytes, false, false);
        }
    }
}