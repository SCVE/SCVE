namespace SCVE.Editor.Editing
{
    public abstract class Clip
    {
        public int StartFrame { get; set; }

        public int FrameLength { get; set; }

        public int EndFrame => StartFrame + FrameLength;

        public virtual string ShortName()
        {
            return "Base clip";
        }
    }
}