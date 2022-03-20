namespace SCVE.Editor.Abstractions
{
    public interface IFileDropReceiver
    {
        void OnFileDrop(string[] paths);
    }
}