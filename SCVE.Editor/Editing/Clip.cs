using System;
using System.Collections.Generic;
using SCVE.Editor.Effects;

namespace SCVE.Editor.Editing
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

        public Track Track { get; set; }
        public int EndFrame => StartFrame + FrameLength;

        public IReadOnlyList<IEffect> Effects => _effects;

        private List<IEffect> _effects;

        protected Clip(Guid guid, int startFrame, int frameLength)
        {
            Guid        = guid;
            StartFrame  = startFrame;
            FrameLength = frameLength;
            _effects     = new List<IEffect>();
        }

        public void AddEffect(IEffect effect)
        {
            effect.AttachToClip(this);
            _effects.Add(effect);
        }

        public virtual string ShortName()
        {
            return "Base clip";
        }

        public void RemoveEffect(int index)
        {
            _effects[index].DeAttachFromClip();
            _effects.RemoveAt(index);
        }
    }
}