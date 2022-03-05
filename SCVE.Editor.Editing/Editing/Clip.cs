using SCVE.Editor.Editing.Effects;

namespace SCVE.Editor.Editing.Editing
{
    public abstract class Clip
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// Track-local Id of the clip
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Absolute index of start frame
        /// </summary>
        public int StartFrame { get; set; }

        public int FrameLength { get; set; }

        public int EndFrame => StartFrame + FrameLength;

        public IReadOnlyList<EffectBase> Effects => _effects;

        private List<EffectBase> _effects;

        protected internal Clip()
        {
        }

        protected internal Clip(Guid guid, int startFrame, int frameLength)
        {
            Guid = guid;
            StartFrame = startFrame;
            FrameLength = frameLength;
            _effects = new List<EffectBase>();
        }

        public virtual string ShortName()
        {
            return "Base clip";
        }
    }
}