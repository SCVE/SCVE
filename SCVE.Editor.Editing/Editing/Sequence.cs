using SCVE.Editor.Editing.Misc;

namespace SCVE.Editor.Editing.Editing
{
    public class Sequence
    {
        public Guid Guid { get; set; }

        public IList<Track> Tracks { get; set; }

        public int FPS { get; set; }

        public int CursorTimeFrame { get; set; }

        public ScveVector2i Resolution { get; set; }


        // Sequence length, independent of it's content
        public int FrameLength { get; set; }

        public Sequence()
        {
        }

        private Sequence(Guid guid, int fps, ScveVector2i resolution)
        {
            FPS = fps;
            Guid = guid;
            Resolution = resolution;
            Tracks = new List<Track>();
        }

        public static Sequence CreateNew(int fps, ScveVector2i resolution)
        {
            return new(Guid.NewGuid(), fps, resolution);
        }
    }
}