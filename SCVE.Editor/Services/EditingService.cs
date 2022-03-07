using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class EditingService : IService
    {
        public VideoProject OpenedProject { get; private set; }
        public Sequence OpenedSequence { get; private set; }
        public Clip SelectedClip { get; private set; }

        public EditingService()
        {
        }

        public void SetOpenedProject(VideoProject project)
        {
            OpenedProject = project;
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

        public void OnUpdate()
        {
        }

        public void AddSequence(SequenceAsset sequenceAsset)
        {
            OpenedProject.AddSequence(sequenceAsset);
        }
    }
}