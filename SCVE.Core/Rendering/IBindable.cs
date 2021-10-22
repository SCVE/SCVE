namespace SCVE.Core.Rendering
{
    /// <summary>
    /// Represents an object, which can be bound and unbound
    /// </summary>
    public interface IBindable
    {
        void Bind();
        
        void Unbind();
    }
}