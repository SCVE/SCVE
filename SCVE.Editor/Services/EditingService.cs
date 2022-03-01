using SCVE.Editor.Editing;
using SCVE.Editor.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class EditingService : IService
    {
        public VideoProject OpenedProject { get; private set; }
        public Sequence OpenedSequence { get; private set; }
        public Clip SelectedClip { get; set; }

        public EditingService()
        {
            if (Project.PathIsProject("testdata/projects/abc.scve"))
            {
                Utils.DeleteDummyProject("abc", "testdata/projects/");
            }

            Utils.CreateDummyProject("abc", "testdata/projects/");

            OpenedProject = Project.LoadFrom("testdata/projects/abc.scve");

            OpenedSequence = Utils.CreateTestingSequence();
        }

        public void OnUpdate()
        {
        }
    }
}