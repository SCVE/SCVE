using System;

namespace SCVE.Editor.Editing
{
    public class EmptyClip : Clip
    {
        private EmptyClip(Guid guid, int startFrame, int frameLength) : base(guid, startFrame, frameLength)
        {
        }

        public static EmptyClip CreateNew(int startFrame, int frameLength)
        {
            return new(Guid.NewGuid(), startFrame, frameLength);
        }

        public override string ShortName()
        {
            return "Empty Clip";
        }
    }
}