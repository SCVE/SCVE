using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class SequenceInfoPanel : IImGuiPanel
    {
        private readonly EditingService _editingService;

        public SequenceInfoPanel(EditingService editingService)
        {
            _editingService = editingService;
        }

        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Sequence Info Panel"))
            {
                goto END;
            }

            if (_editingService.OpenedSequence is null)
            {
                ImGui.Text("No sequence is opened");
                goto END;
            }

            ImGui.Text($"GUID: {_editingService.OpenedSequence.Guid}");
            ImGui.Text($"FrameLength: {_editingService.OpenedSequence.FrameLength}");
            ImGui.Text($"CursorTimeFrame: {_editingService.OpenedSequence.CursorTimeFrame}");
            ImGui.Text($"FPS: {_editingService.OpenedSequence.FPS}");
            ImGui.Text($"Resolution: {_editingService.OpenedSequence.Resolution}");

            END:
            ImGui.End();
        }
    }
}