using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.Core.Lifecycle
{
    /// <summary>
    /// Represents any entity, that can be rendered by the engine
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Render call from the engine
        /// </summary>
        void Render(IRenderer renderer);
    }
}