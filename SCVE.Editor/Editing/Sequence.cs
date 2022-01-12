using System.Collections.Generic;
using System.Linq;

namespace SCVE.Editor.Editing
{
    public class Sequence
    {
        public List<Track> Tracks;

        public Sequence()
        {
            Tracks = new List<Track>();
        }

        public int FrameLength
        {
            get
            {
                if (Tracks.Count == 0) return 0;
                else return Tracks.Max(t => t.FrameLength);
            }
        }
    }
}