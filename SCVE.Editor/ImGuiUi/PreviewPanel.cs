using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class PreviewPanel : IImGuiRenderable
    {
        public void OnImGuiRender()
        {
            if (ImGui.Begin("Preview Panel"))
            {
                var windowSize = ImGui.GetWindowSize();

                var imageSize = new Vector2(400, 400);
                
                ImGui.SetCursorPos((windowSize - imageSize) * 0.5f);

                ImGui.Button("CLICK ME", imageSize);
                
                ImGui.End();
            }
        }
    }
}