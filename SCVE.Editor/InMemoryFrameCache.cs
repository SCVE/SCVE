using System.Collections.Generic;
using SCVE.Editor.Effects;

namespace SCVE.Editor
{
    public class InMemoryFrameCache
    {
        public Dictionary<int, ImageFrame> Frames { get; set; }

        public InMemoryFrameCache()
        {
            Frames = new();
        }

        public void AddSampledFrame(int index, ImageFrame frame)
        {
            Frames[index] = frame;
        }

        public void InvalidateSampledFrame(int index)
        {
            if (Frames.ContainsKey(index))
            {
                Frames[index].Dispose();
                Frames.Remove(index);
            }
        }
    }
}