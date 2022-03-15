using System;
using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json;

namespace SCVE.Editor
{
    public class Settings
    {
        public static Settings Instance => _instance;
        private static Settings _instance;

        public int TrackMarginLeft { get; set; }
        public int TrackHeaderWidth { get; set; }

        public int TrackHeight { get; set; }
        public int TrackMargin { get; set; }

        public int SequenceHeaderHeight { get; set; }

        public int TimelineFrameMarkerHeight { get; set; }
        public int TimelineSecondsMarkerHeight { get; set; }

        public Vector2 CursorSize { get; set; }

        [JsonIgnore] public Vector2[] CursorShapePoints { get; set; }
        [JsonIgnore] public Vector2[] CursorCurrentPoints { get; set; }

        // Do not delete because JSON serialization.
        // ReSharper disable once EmptyConstructor
        public Settings()
        {
            
        }

        public void CompleteInnerCalculations()
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

        public static void SetFrom(Settings settings)
        {
            _instance = settings;
            _instance.CompleteInnerCalculations();
        }

        public static void SetFromDefault()
        {
            _instance = new Settings()
            {
                TrackMarginLeft = 10,
                TrackHeaderWidth = 70,

                TrackHeight = 20,
                TrackMargin = 5,

                SequenceHeaderHeight = 20,

                TimelineFrameMarkerHeight = 3,
                TimelineSecondsMarkerHeight = 8,

                CursorSize = new(10, 20)
            };
            _instance.CompleteInnerCalculations();
        }
    }
}