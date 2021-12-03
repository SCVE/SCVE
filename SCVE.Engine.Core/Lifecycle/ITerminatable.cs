namespace SCVE.Engine.Core.Lifecycle
{
    /// <summary>
    /// Represents any entity, that needs to be terminated by the engine
    /// </summary>
    public interface ITerminatable
    {
        void OnTerminate();
    }
}