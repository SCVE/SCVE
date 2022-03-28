using System;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Late
{
    public class OpenProjectLateTask : LateTask
    {
        private VideoProject _videoProject;
        private string _path;

        public OpenProjectLateTask(VideoProject videoProject, string path)
        {
            _videoProject = videoProject;
            _path = path;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.SetOpenedProject(_videoProject, _path);
            visitor.RecentsService.NoticeOpen(_path);
            visitor.ProjectPanelService.ChangeLocation("/");
            visitor.PreviewService.SwitchToNone();

            Console.WriteLine($"Executed late action: OpenProject");
        }
    }
}