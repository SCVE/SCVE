using System;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Late
{
    public class AddImageLateTask : LateTask
    {
        private ImageAsset _imageAsset;

        public AddImageLateTask(ImageAsset imageAsset)
        {
            _imageAsset = imageAsset;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.OpenedProject.AddImage(_imageAsset);
            visitor.ProjectPanelService.RescanCurrentLocation();
            Console.WriteLine($"Executed late action: AddImage");
        }
    }
}