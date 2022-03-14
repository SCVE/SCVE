namespace SCVE.Editor.Editing.Editing
{
    public class GhostClip
    {
        public bool Visible;

        public Clip ReferencedClip { get; set; }

        public int SourceStartFrame { get; set; }
        public int SourceTrackIndex { get; set; }
        public int SourceFrameLength { get; set; }

        public int CurrentTrackIndex { get; set; }
        public int CurrentStartFrame { get; set; }
        public int CurrentFrameLength { get; set; }

        public GhostClip()
        {
        }

        public string ShortName()
        {
            return "Ghost";
        }
    }
}