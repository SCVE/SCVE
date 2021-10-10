namespace SCVE.Core.App
{
    public enum AppState
    {
        Uninitialized,
        Starting,
        Ready,
        Running,
        TerminationRequested,
        Terminating,
        Terminated
    }
}