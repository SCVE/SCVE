namespace SCVE.Core.Lifecycle
{
    /// <summary>
    /// Represents any entity, that need to be initiated by engine
    /// </summary>
    public interface IInitable
    {
        /// <summary>
        /// Initialize call from engine
        /// </summary>
        public void Init();
    }
}