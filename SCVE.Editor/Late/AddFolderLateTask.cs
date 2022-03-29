using System;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Late
{
    public class AddFolderLateTask : LateTask
    {
        private FolderAsset _folderAsset;

        public AddFolderLateTask(FolderAsset folderAsset)
        {
            _folderAsset = folderAsset;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.EditingService.AddFolder(_folderAsset);
            visitor.ProjectPanelService.RescanCurrentLocation();
            Console.WriteLine($"Executed late action: AddFolder");
        }
    }
}