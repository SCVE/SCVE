using System;
using System.Collections.Generic;
using System.Linq;

namespace SCVE.Editor.Editing
{
    public class Sequence
    {
        public Guid Guid { get; set; }

        public IReadOnlyList<Track> Tracks => _tracks;
        private List<Track> _tracks;

        public int FPS { get; private set; }

        private Sequence(Guid guid, int fps)
        {
            _tracks = new List<Track>();
            FPS     = fps;
            Guid    = guid;
        }

        public static Sequence CreateNew(int fps)
        {
            return new(Guid.NewGuid(), fps);
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