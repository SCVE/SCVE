using System;

namespace SCVE.Editor.Editing
{
    public abstract class Clip
    {
        public Guid Guid { get; set; }

        /// <summary>
        /// Track-local Id of the clip
        /// </summary>
        public int Id { get; set; }

        public int StartFrame { get; set; }

        public int FrameLength { get; set; }

        public Track Track { get; set; }
        public int EndFrame => StartFrame + FrameLength;

        protected Clip(Guid guid, int startFrame, int frameLength)
        {
            Guid        = guid;
            StartFrame  = startFrame;
            FrameLength = frameLength;
        }

        public virtual string ShortName()
        {
            return "Base clip";
        }
    }
}