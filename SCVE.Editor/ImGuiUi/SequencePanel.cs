using System;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Imaging;
using SCVE.Editor.ImGuiUi.Services;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanel : IImGuiRenderable
    {
        #region Fields

        private readonly EditingService _editingService;
        private readonly PreviewService _previewService;


        private readonly SequencePanelService _sequencePanelService;

        #endregion

        public SequencePanel(EditingService editingService, PreviewService previewService, SequencePanelService sequencePanelService)
        {
            _editingService = editingService;
            _previewService = previewService;
            _sequencePanelService = sequencePanelService;
        }


        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Sequence Panel"))
            {
                goto END;
            }

            if (_editingService.OpenedSequence is null)
            {
                ImGui.Text("No sequence is opened");
                goto END;
            }

            _sequencePanelService.RefreshData();

            _sequencePanelService.DrawSequenceHeader();

            _sequencePanelService.DrawCursor();

            _sequencePanelService.DrawTracks();
                
            _sequencePanelService.DrawGhostClip();

            END:
            ImGui.End();
        }
    }
}