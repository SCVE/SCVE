using System.Numerics;

namespace SCVE.Editor
{
    public class Settings
    {
        public /*static readonly*/ int TrackMarginLeft = 10;
        public /*static readonly*/ int TrackHeaderWidth = 70;

        public /*static readonly*/ int TrackHeight = 20;
        public /*static readonly*/ int TrackMargin = 5;

        public /*static readonly*/ int SequenceHeaderHeight = 20;

        public /*static readonly*/ int TimelineFrameMarkerHeight = 3;
        public /*static readonly*/ int TimelineSecondsMarkerHeight = 8;

        public /*readonly*/ Vector2 CursorSize = new(10, 20);
        public /*readonly*/ Vector2[] CursorShapePoints;
        public /*readonly*/ Vector2[] CursorCurrentPoints;

        public Settings()
        {
            CursorShapePoints = new[]
            {
                // top-left
                new Vector2(),
                // top-right
                new(CursorSize.X, 0),
                // right mid
                new(CursorSize.X,
                    CursorSize.Y / 2f),
                // bottom mid
                new(CursorSize.X / 2, CursorSize.Y),
                // left mid
                new(0, CursorSize.Y / 2f),
            };
            CursorCurrentPoints = new Vector2[CursorShapePoints.Length];
        }
    }
}