using System.Linq;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class SequenceCreationPanel : IImGuiRenderable
    {
        public bool Visible { get; set; }

        public void OnImGuiRender()
        {
            if (!Visible)
            {
                return;
            }

            ImGui.OpenPopup("New Sequence");
            bool _visible = Visible;
            if (ImGui.BeginPopupModal("New Sequence", ref _visible))
            {
                Visible = _visible;
                ImGui.TextDisabled($"New sequence");

                if (ImGui.Button("Close"))
                {
                    Visible = false;
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }
            else
            {
                Visible = false;
                ImGui.CloseCurrentPopup();
            }

            END:
            ImGui.End();
        }
    }
}