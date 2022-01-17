using System.Numerics;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class SequenceInfoPanel : IImGuiRenderable
    {
        public void OnImGuiRender()
        {
            if (ImGui.Begin("Sequence Info Panel"))
            {
                if (EditorApp.Instance.OpenedSequence is null)
                {
                    ImGui.Text("No sequence is opened");
                    ImGui.End();
                    return;
                }
                
                ImGui.Text($"GUID: {EditorApp.Instance.OpenedSequence.Guid}");
                ImGui.Text($"FrameLength: {EditorApp.Instance.OpenedSequence.FrameLength}");
                ImGui.Text($"MaxFrame: {EditorApp.Instance.OpenedSequence.MaxFrame}");
                ImGui.Text($"CursorTimeFrame: {EditorApp.Instance.OpenedSequence.CursorTimeFrame}");
                ImGui.Text($"FPS: {EditorApp.Instance.OpenedSequence.FPS}");
                ImGui.Text($"Resolution: {EditorApp.Instance.OpenedSequence.Resolution}");
                
                ImGui.End();
            }
        }
    }
}