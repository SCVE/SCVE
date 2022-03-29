using System;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Late
{
    public class LoadPreviewImageLateTask : LateTask
    {
        private ThreeWayImage _image;

        public LoadPreviewImageLateTask(ThreeWayImage image)
        {
            _image = image;
        }

        public override void AcceptVisitor(LateTaskVisitor visitor)
        {
            visitor.PreviewService.SwitchToImage(_image);
            Console.WriteLine($"Executed late action: LoadPreview");
        }
    }
}