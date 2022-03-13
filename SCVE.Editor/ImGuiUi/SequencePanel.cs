using System;
using ImGuiNET;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Imaging;
using SCVE.Editor.Services;

namespace SCVE.Editor.ImGuiUi
{
    public class SequencePanel : IImGuiRenderable
    {
        #region Fields

        private ClipImGuiRenderer _clipRenderer;

        private readonly EditingService _editingService;
        private readonly PreviewService _previewService;


        private readonly GhostClip _ghostClip;

        private readonly SequencePanelService _sequencePanelService;

        #endregion

        public SequencePanel(EditingService editingService, PreviewService previewService, SequencePanelService sequencePanelService)
        {
            _editingService = editingService;
            _previewService = previewService;
            _sequencePanelService = sequencePanelService;
            _clipRenderer = new ClipImGuiRenderer();

            _ghostClip = GhostClip.CreateNew(0, 1);
            
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

            _sequencePanelService.DetectClickOnTimeline();

            _sequencePanelService.DrawSequenceHeader();

            _sequencePanelService.DrawSequenceFramesMarkers();

            _sequencePanelService.DrawCursor();

            _sequencePanelService.DrawTracks(_clipRenderer, _ghostClip);
                
            _sequencePanelService.ProcessGhostClip(_clipRenderer, _ghostClip);
            _sequencePanelService.ProcessDraggingClip(_ghostClip);
            _sequencePanelService.ProcessDraggingCursor();
            

            END:
            ImGui.End();
        }
    }
}