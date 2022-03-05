using System.Collections;
using System.Numerics;

namespace SCVE.Editor.Editing.Editing
{
    public class Sequence
    {
        public Guid Guid { get; private set; }

        public IList<Track> Tracks { get; set; }

        public int FPS { get; set; }

        public int CursorTimeFrame { get; set; }

        public Vector2 Resolution { get; set; }


        // Sequence length, independent of it's content
        public int FrameLength { get; set; }

        public Sequence()
        {
        }

        private Sequence(Guid guid, int fps, Vector2 resolution)
        {
            FPS = fps;
            Guid = guid;
            Resolution = resolution;
        }

        public static Sequence CreateNew(int fps, Vector2 resolution)
        {
            return new(Guid.NewGuid(), fps, resolution);
        }
    }
}