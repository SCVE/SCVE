using System;
using SCVE.Editor.Abstractions;
using SCVE.Editor.ImGuiUi;
using Silk.NET.Input;

namespace SCVE.Editor.Services
{
    public class ModalManagerService : IService
    {
        private readonly ProjectCreationPanel _projectCreationPanel;
        private readonly SequenceCreationPanel _sequenceCreationPanel;
        private readonly FilePickerModalPanel _filePickerModalPanel;

        public ModalManagerService(ProjectCreationPanel projectCreationPanel, SequenceCreationPanel sequenceCreationPanel, FilePickerModalPanel filePickerModalPanel)
        {
            _projectCreationPanel = projectCreationPanel;
            _sequenceCreationPanel = sequenceCreationPanel;
            _filePickerModalPanel = filePickerModalPanel;
        }

        public string FilePickerSelectedPath => _filePickerModalPanel.SelectedPath;

        public void OpenProjectCreationPanel(Action closed = null, Action dismissed = null)
        {
            _projectCreationPanel.Open(closed, dismissed);
        }

        public void OpenSequenceCreationPanel(Action closed = null, Action dismissed = null)
        {
            _sequenceCreationPanel.Open(closed, dismissed);
        }

        public void OpenFilePickerPanel(string location, Action closed = null, Action dismissed = null)
        {
            _filePickerModalPanel.Open(location, closed, dismissed);
        }
    }
}