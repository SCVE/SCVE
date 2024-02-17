using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ClipEffectsPanel : IImGuiPanel
    {
        public ClipEffectsPanel()
        {
        }

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Clip Effects"))
            {
                ImGui.End();
            }
        }
    }
}