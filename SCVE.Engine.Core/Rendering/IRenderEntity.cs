namespace SCVE.Engine.Core.Rendering
{
    /// <summary>
    /// Any entity, that can be instanced by Graphics Engine (aka OpenGL) has an Id
    /// </summary>
    public interface IRenderEntity
    {
        public int Id { get; }
    }
}