using System.Linq;
using System.Numerics;
using Newtonsoft.Json;

namespace SCVE.Editor
{
    public class Settings
    {
        public static Settings Instance => _instance;

        private static Settings _instance;

        public static Settings Default
        {
            get
            {
                if (_default is not null) return _default;

                _default = new()
                {
                    Version = ActualVersion,

                    ClipPadding = 3,
                    ClipRounding = 3,

                    TrackMarginLeft = 10,
                    TrackHeaderWidth = 70,

                    TrackHeight = 20,
                    TrackMargin = 5,

                    SequenceHeaderHeight = 20,

                    TimelineFrameMarkerHeight = 3,
                    TimelineSecondsMarkerHeight = 8,

                    CursorSize = new(10, 20)
                };
                _default.CompleteInnerCalculations();

                return _default;
            }
        }

        private static Settings _default;

        public const int ActualVersion = 1;

        public int Version { get; set; }

        public int ClipPadding { get; set; }
        public int ClipRounding { get; set; }

        public int TrackMarginLeft { get; set; }
        public int TrackHeaderWidth { get; set; }

        public int TrackHeight { get; set; }
        public int TrackMargin { get; set; }

        public int SequenceHeaderHeight { get; set; }

        public int TimelineFrameMarkerHeight { get; set; }
        public int TimelineSecondsMarkerHeight { get; set; }

        public Vector2 CursorSize { get; set; }

        [JsonIgnore] public Vector2[] CursorShapePoints { get; set; }

        [JsonConstructor]
        private Settings()
        {
        }

        private Settings(Settings settings)
        {
            Version = settings.Version;
            ClipPadding = settings.ClipPadding;
            ClipRounding = settings.ClipRounding;
            TrackMarginLeft = settings.TrackMarginLeft;
            TrackHeaderWidth = settings.TrackHeaderWidth;
            TrackHeight = settings.TrackHeight;
            TrackMargin = settings.TrackMargin;
            SequenceHeaderHeight = settings.SequenceHeaderHeight;
            TimelineFrameMarkerHeight = settings.TimelineFrameMarkerHeight;
            TimelineSecondsMarkerHeight = settings.TimelineSecondsMarkerHeight;
            CursorSize = settings.CursorSize;

            // ToArray implicitly duplicates all elements
            CursorShapePoints = settings.CursorShapePoints.ToArray();
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
                new(CursorSize.X, CursorSize.Y / 2f),
                // bottom mid
                new(CursorSize.X / 2, CursorSize.Y),
                // left mid
                new(0, CursorSize.Y / 2f),
            };
        }

        public static void SetFrom(Settings settings)
        {
            _instance = settings;
            _instance.CompleteInnerCalculations();
        }

        public static void SetFromCopy(Settings settings)
        {
            _instance = new Settings(settings);
            _instance.CompleteInnerCalculations();
        }

        public static Settings GetClone()
        {
            return new Settings(_instance);
        }
    }
}