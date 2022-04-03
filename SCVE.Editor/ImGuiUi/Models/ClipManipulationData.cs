using System.Drawing;

namespace SCVE.Editor.ImGuiUi.Models
{
    public class ClipManipulationData
    {
        public bool IsLeftClicked;
        public bool IsBodyClicked;

        public bool IsRightClicked;

        public bool IsLeftActive;
        public bool IsBodyActive;
        public bool IsRightActive;

        public int LeftDragDeltaFrames;
        public int BodyDragDeltaFrames;
        public int RightDragDeltaFrames;
        public int DeltaTracks;

        public void Reset()
        {
            IsLeftClicked = false;
            IsBodyClicked = false;
            IsRightClicked = false;
            IsLeftActive = false;
            IsBodyActive = false;
            IsRightActive = false;
            LeftDragDeltaFrames = 0;
            BodyDragDeltaFrames = 0;
            RightDragDeltaFrames = 0;
            DeltaTracks = 0;
        }
        
        public bool IsAnyPartClicked()
        {
            return IsLeftClicked || IsBodyClicked || IsRightClicked;
        }
    }
}