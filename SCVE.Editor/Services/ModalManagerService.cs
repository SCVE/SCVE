using System;
using System.Threading.Channels;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.ImGuiUi.Panels;

namespace SCVE.Editor.Services
{
    public class ModalManagerService : IService
    {
        private readonly ProjectCreationPanel _projectCreationPanel;
        private readonly SequenceCreationPanel _sequenceCreationPanel;
        private readonly FolderCreationPanel _folderCreationPanel;
        private readonly SettingsModalPanel _settingsModalPanel;
        
        private ExportSequenceModalPanel _exportSequenceModalPanel;


        public ModalManagerService(
            ProjectCreationPanel projectCreationPanel,
            SequenceCreationPanel sequenceCreationPanel,
            SettingsModalPanel settingsModalPanel, 
            FolderCreationPanel folderCreationPanel, 
            ExportSequenceModalPanel exportSequenceModalPanel)
        {
            _projectCreationPanel = projectCreationPanel;
            _sequenceCreationPanel = sequenceCreationPanel;
            _settingsModalPanel = settingsModalPanel;
            _folderCreationPanel = folderCreationPanel;
            _exportSequenceModalPanel = exportSequenceModalPanel;
        }

        public void OpenProjectCreationPanel(Action closed = null, Action dismissed = null)
        {
            _projectCreationPanel.Open(closed, dismissed);
        }

        public void OpenSequenceCreationPanel(Action closed = null, Action dismissed = null)
        {
            _sequenceCreationPanel.Open(closed, dismissed);
        }

        public void OpenSettingsPanel(Action closed = null, Action dismissed = null)
        {
            _settingsModalPanel.LoadDraft();
            _settingsModalPanel.Open(closed, dismissed);
        }

        public void OpenFolderCreationPanel(Action closed = null, Action dismissed = null)
        {
            _folderCreationPanel.Open(closed, dismissed);
        }

        public void OpenExportSequencePanel(Sequence sequence)
        {
            _exportSequenceModalPanel.Open(sequence);
        }
    }
}