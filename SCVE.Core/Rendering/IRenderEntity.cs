namespace SCVE.Core.Rendering
{
    /// <summary>
    /// Any entity, that can be instanced by Graphics Engine (aka OpenGL) implement an Id property
    /// </summary>
    public interface IRenderEntity
    {
        public int Id { get; }
    }
}