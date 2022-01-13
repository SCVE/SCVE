using System.Collections.Generic;
using System.Linq;

namespace SCVE.Editor.Editing
{
    // Track is a single line, existent for the whole length of the sequence
    public class Track
    {
        public List<Clip> Clips;

        public Track()
        {
            Clips = new List<Clip>();
        }

        public int StartFrame
        {
            get
            {
                if (Clips.Count == 0) return 0;
                else return Clips.Min(c => c.StartFrame);
            }
        }

        public int EndFrame
        {
            get
            {
                if (Clips.Count == 0) return 0;
                else return Clips.Max(c => c.EndFrame);
            }
        }
    }
}