using System.Text.Json.Serialization;

namespace SCVE.Editor.Editing.Editing
{
    /// <summary>
    /// Track is a single line, existent for the whole length of the sequence
    /// </summary>
    public class Track
    {
        public Guid Guid { get; set; }

        public IList<EmptyClip> EmptyClips { get; set; }
        public IList<AssetClip> AssetClips { get; set; }

        
        [JsonConstructor]
        private Track()
        {
        }

        public static Track CreateNew()
        {
            return new()
            {
                Guid = Guid.NewGuid(),
                EmptyClips = new List<EmptyClip>(),
                AssetClips = new List<AssetClip>()
            };
        }
    }
}