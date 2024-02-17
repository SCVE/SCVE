namespace SCVE.Editor.FileSystem;

public class ProjectFile
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<SequenceData> Sequences { get; set; } = new();
    public List<TrackData> Tracks { get; set; } = new();
    public List<ClipData> Clips { get; set; } = new();
}