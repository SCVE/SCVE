using SCVE.Editor.Editing;
using SCVE.Editor.ProjectStructure;
using Silk.NET.Vulkan;

namespace SCVE.Editor.Modules
{
    public class EditingModule : IModule
    {
        public Project OpenedProject { get; private set; }
        public Sequence OpenedSequence { get; private set; }
        public Clip SelectedClip { get; set; }

        public void CrossReference(ModulesContainer modulesContainer)
        {
        }

        public void OnInit()
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