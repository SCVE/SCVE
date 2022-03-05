namespace SCVE.Editor.Editing.Editing
{
    public class GhostClip : Clip
    {
        public bool Visible;

        public Clip ReferencedClip { get; set; }

        public int SourceStartFrame { get; set; }

        public int SourceTrackIndex { get; set; }
        public int CurrentTrackIndex { get; set; }

        public GhostClip()
        {
            
        }
        
        private GhostClip(int startFrame, int frameLength) : base(Guid.Empty, startFrame, frameLength)
        {
        }

        public static GhostClip CreateNew(int startFrame, int frameLength)
        {
            return new(startFrame, frameLength);
        }

        public override string ShortName()
        {
            return "Ghost";
        }
    }
}