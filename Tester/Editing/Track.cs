namespace Tester.Editing
{
    // Track is a single line, existent for the whole length of the sequence
    public class Track
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }

        public IReadOnlyList<Clip> Clips => _clips;
        private List<Clip> _clips = new();

        private Track(Guid guid)
        {
        }

        public static Track CreateNew()
        {
            return new(Guid.NewGuid());
        }

        public void AddClip(Clip clip)
        {
            clip.Track = this;
            clip.Id    = Clips.Count;
            _clips.Add(clip);
        }

        public int StartFrame
        {
            get
            {
                if (_clips.Count == 0) return 0;
                else return _clips.Min(c => c.StartFrame);
            }
        }

        public int EndFrame
        {
            get
            {
                if (_clips.Count == 0) return 0;
                else return _clips.Max(c => c.EndFrame);
            }
        }
    }
}