using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Misc;

namespace SCVE.Editor.Editing.Editing
{
    public class Sequence
    {
        public Guid Guid { get; set; }

        public IList<Track> Tracks { get; set; }

        public int FPS { get; set; }

        public ScveVector2I Resolution { get; set; }
        
        // Sequence length, independent of it's content
        public int FrameLength { get; set; }

        [JsonConstructor]
        private Sequence()
        {
        }

        private Sequence(Guid guid, int fps, ScveVector2I resolution, int frameLength)
        {
            FPS = fps;
            Guid = guid;
            Resolution = resolution;
            FrameLength = frameLength;
            Tracks = new List<Track>();
        }

        public static Sequence CreateNew(int fps, ScveVector2I resolution, int frameLength)
        {
            return new(Guid.NewGuid(), fps, resolution, frameLength);
        }
    }
}