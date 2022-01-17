using System;

namespace SCVE.Editor.Editing
{
    public class GhostClip : Clip
    {
        public bool Visible;

        private GhostClip(Guid guid, int startFrame, int frameLength) : base(guid, startFrame, frameLength)
        {
        }

        public static GhostClip CreateNew(int startFrame, int frameLength)
        {
            return new(Guid.NewGuid(), startFrame, frameLength);
        }

        public override string ShortName()
        {
            return "Ghost";
        }
    }
}