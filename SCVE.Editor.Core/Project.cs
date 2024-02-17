namespace SCVE.Editor.Core;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Title { get; set; }

    public Project(string title)
    {
        Title = title;
    }
}