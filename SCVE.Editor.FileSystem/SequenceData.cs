namespace SCVE.Editor.FileSystem;

public class SequenceData
{
    public Guid Id { get; set; }
    
    public Guid ProjectId { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public int Fps { get; set; }
}