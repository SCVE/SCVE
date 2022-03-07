using ImGuiNET;
using SCVE.Editor.ImGuiUi;

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

        public void OpenProjectCreationPanel()
        {
            _projectCreationPanel.Open();
        }

        public void OpenSequenceCreationPanel()
        {
            _sequenceCreationPanel.Open();
        }

        public void OpenFilePickerPanel(string location)
        {
            _filePickerModalPanel.Open(location);
        }

        public void OnUpdate()
        {
        }
    }
}