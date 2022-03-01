using System.Numerics;

namespace Tester.Editing
{
    public class Sequence
    {
        public Guid Guid { get; private set; }

        public IReadOnlyList<Track> Tracks => _tracks;
        private List<Track> _tracks;

        public int FPS { get; set; }

        public int CursorTimeFrame { get; set; }

        public Vector2 Resolution { get; set; }

        public Sequence()
        {
        }

        private Sequence(Guid guid, int fps, Vector2 resolution)
        {
            _tracks = new List<Track>();
            FPS = fps;
            Guid = guid;
            Resolution = resolution;
        }

        public static Sequence CreateNew(int fps, Vector2 resolution)
        {
            return new(Guid.NewGuid(), fps, resolution);
        }

        public void AddTrack(Track track)
        {
            track.Id = _tracks.Count;
            _tracks.Add(track);
        }

        // Sequence length, independent of it's content
        public int FrameLength { get; set; }

        /// <summary>
        /// Content max frame
        /// </summary>
        public int MaxFrame
        {
            get
            {
                if (_tracks.Count == 0) return 0;
                else return _tracks.Max(t => t.EndFrame);
            }
        }
    }
}