using System.Text.Json.Serialization;
using SCVE.Editor.Editing.Effects;

namespace SCVE.Editor.Editing.Editing
{
    public abstract class Clip
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// Absolute index of start frame
        /// </summary>
        public int StartFrame { get; set; }

        public int FrameLength { get; set; }

        [JsonIgnore]
        public int EndFrame => StartFrame + FrameLength;

        public IList<EffectBase> Effects { get; set; }

        protected internal Clip()
        {
        }

        protected internal Clip(Guid guid, int startFrame, int frameLength)
        {
            Guid = guid;
            StartFrame = startFrame;
            FrameLength = frameLength;
            Effects = new List<EffectBase>();
        }

        public virtual string ShortName()
        {
            return "Base clip";
        }
    }
}