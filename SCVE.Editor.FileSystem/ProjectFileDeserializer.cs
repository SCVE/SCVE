using System.Text.Json;

namespace SCVE.Editor.FileSystem;

public class ProjectFileDeserializer : IProjectFileDeserializer
{
    public ProjectFile? Deserialize(Stream stream)
    {
        return JsonSerializer.Deserialize<ProjectFile>(stream);
    }
}