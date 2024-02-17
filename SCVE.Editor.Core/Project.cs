namespace SCVE.Editor.Core;

public class Project
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Title { get; set; }
}