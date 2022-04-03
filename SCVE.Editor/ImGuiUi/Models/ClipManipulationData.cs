using System.Drawing;

namespace SCVE.Editor.ImGuiUi.Models
{
    public struct ClipManipulationData
    {
        public bool IsHeadClicked;
        public bool IsBodyClicked;
        public bool IsTailClicked;

        public bool IsHeadActive;
        public bool IsBodyActive;
        public bool IsTailActive;

        public int HeadDragDeltaFrames;
        public int BodyDragDeltaFrames;
        public int TailDragDeltaFrames;

        public int DeltaTracks;

        public bool IsAnyPartClicked()
        {
            return IsHeadClicked || IsBodyClicked || IsTailClicked;
        }

        public bool IsAnyPartActive()
        {
            return IsHeadActive || IsBodyActive || IsTailActive;
        }
    }
}