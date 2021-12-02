namespace SCVE.Engine.Core.Lifecycle
{
    /// <summary>
    /// Represents any entity, that need to receive update loop calls from the engine
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Update from the engine, called every frame
        /// </summary>
        void Update(float deltaTime);
    }
}