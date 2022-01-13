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

        // Sequence length, independent of it's content
        public int FrameLength { get; set; }

        /// <summary>
        /// Content max frame
        /// </summary>
        public int MaxFrame
        {
            get
            {
                if (Tracks.Count == 0) return 0;
                else return Tracks.Max(t => t.EndFrame);
            }
        }
    }
}