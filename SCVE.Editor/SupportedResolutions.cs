using System.Collections.Generic;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Misc;

namespace SCVE.Editor
{
    public class Resolution : IVisible
    {
        public string VisibleTitle { get; }

        public ScveVector2I Value { get; }

        public Resolution(string visibleTitle, ScveVector2I value)
        {
            VisibleTitle = visibleTitle;
            Value = value;
        }
    }

    public class SupportedResolutions
    {
        public static IReadOnlyList<Resolution> Resolutions = new List<Resolution>
        {
            new("1920x1080", new ScveVector2I(1920, 1080)),
            new("1600x900", new ScveVector2I(1600, 900)),
            new("1280x720", new ScveVector2I(1280, 720)),
            new("640x480", new ScveVector2I(640, 480))
        };
    }
}