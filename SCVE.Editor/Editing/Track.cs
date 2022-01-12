using System.Collections.Generic;
using System.Linq;

namespace SCVE.Editor.Editing
{
    public class Track
    {
        public List<Clip> Clips;

        public Track()
        {
            Clips = new List<Clip>();
        }

        public int FrameLength
        {
            get
            {
                if (Clips.Count == 0) return 0;
                else return Clips.Max(c => c.StartFrame + c.FrameLength);
            }
        }
    }
}