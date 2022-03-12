using ImGuiNET;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;
using Silk.NET.Input;

namespace SCVE.Editor.Services
{
    public class EditingService : IService
    {
        public VideoProject OpenedProject { get; private set; }
        public Sequence OpenedSequence { get; private set; }
        public Clip SelectedClip { get; private set; }

        public string OpenedProjectPath { get; set; }

        public EditingService()
        {
        }

        public void SetOpenedProject(VideoProject project, string path = null)
        {
            OpenedProject = project;
            OpenedProjectPath = path;
            OpenedSequence = null;
            SelectedClip = null;
        }

        public void SetOpenedSequence(Sequence sequence)
        {
            OpenedSequence = sequence;
            SelectedClip = null;
        }

        public void SetSelectedClip(Clip clip)
        {
            SelectedClip = clip;
        }

        public void AddSequence(SequenceAsset sequenceAsset)
        {
            OpenedProject.AddSequence(sequenceAsset);
        }
    }
}