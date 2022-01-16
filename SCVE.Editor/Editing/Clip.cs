namespace SCVE.Editor.Editing
{
    public abstract class Clip
    {
        /// <summary>
        /// Track-local Id of the clip
        /// </summary>
        public int Id { get; set; }
        
        public int StartFrame { get; set; }

        public int FrameLength { get; set; }

        public Track Track { get; set; }
        public int EndFrame => StartFrame + FrameLength;

        public virtual string ShortName()
        {
            return "Base clip";
        }
    }
}