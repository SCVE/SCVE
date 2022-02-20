using System.Numerics;
using ImGuiNET;
using SCVE.Editor.Modules;

namespace SCVE.Editor.ImGuiUi
{
    public class SequenceInfoPanel : IImGuiRenderable
    {
        private readonly EditingModule _editingModule;

        public SequenceInfoPanel()
        {
            _editingModule = EditorApp.Modules.Get<EditingModule>();
        }

        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Sequence Info Panel"))
            {
                goto END;
            }

            if (_editingModule.OpenedSequence is null)
            {
                ImGui.Text("No sequence is opened");
                goto END;
            }

            ImGui.Text($"GUID: {_editingModule.OpenedSequence.Guid}");
            ImGui.Text($"FrameLength: {_editingModule.OpenedSequence.FrameLength}");
            ImGui.Text($"MaxFrame: {_editingModule.OpenedSequence.MaxFrame}");
            ImGui.Text($"CursorTimeFrame: {_editingModule.OpenedSequence.CursorTimeFrame}");
            ImGui.Text($"FPS: {_editingModule.OpenedSequence.FPS}");
            ImGui.Text($"Resolution: {_editingModule.OpenedSequence.Resolution}");

            END:
            ImGui.End();
        }
    }
}