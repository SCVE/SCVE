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
        
        public bool IsHeadActivated;
        public bool IsBodyActivated;
        public bool IsTailActivated;
        
        public bool IsHeadDeactivated;
        public bool IsBodyDeactivated;
        public bool IsTailDeactivated;

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

        public bool IsAnyPartActivated()
        {
            return IsHeadActivated || IsBodyActivated || IsTailActivated;
        }

        public bool IsAnyPartDeactivated()
        {
            return IsHeadDeactivated || IsBodyDeactivated || IsTailDeactivated;
        }

        public override string ToString()
        {
            return $"{nameof(IsHeadClicked)}: {IsHeadClicked}, {nameof(IsBodyClicked)}: {IsBodyClicked}, {nameof(IsTailClicked)}: {IsTailClicked}, {nameof(IsHeadActive)}: {IsHeadActive}, {nameof(IsBodyActive)}: {IsBodyActive}, {nameof(IsTailActive)}: {IsTailActive}, {nameof(IsHeadActivated)}: {IsHeadActivated}, {nameof(IsBodyActivated)}: {IsBodyActivated}, {nameof(IsTailActivated)}: {IsTailActivated}, {nameof(IsHeadDeactivated)}: {IsHeadDeactivated}, {nameof(IsBodyDeactivated)}: {IsBodyDeactivated}, {nameof(IsTailDeactivated)}: {IsTailDeactivated}, {nameof(HeadDragDeltaFrames)}: {HeadDragDeltaFrames}, {nameof(BodyDragDeltaFrames)}: {BodyDragDeltaFrames}, {nameof(TailDragDeltaFrames)}: {TailDragDeltaFrames}, {nameof(DeltaTracks)}: {DeltaTracks}";
        }
    }
}