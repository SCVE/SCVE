using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class EditingService : IService
    {
        public VideoProject OpenedProject { get; set; }
        public Sequence OpenedSequence { get; set; }
        public Clip SelectedClip { get; set; }

        public EditingService()
        {
        }

        public void OnUpdate()
        {
        }
    }
}