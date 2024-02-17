namespace SCVE.Editor.FileSystem;

public interface IProjectFileDeserializer
{
    ProjectFile? Deserialize(Stream stream);
}