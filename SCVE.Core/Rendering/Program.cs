namespace SCVE.Core.Rendering
{
    public abstract class Program
    {
        public int Id;

        public abstract void AttachShader(Shader shader);
        
        public abstract void DetachShader(Shader shader);

        public abstract void Bind();
        
        public abstract void Unbind();

        public abstract void Link();

        public abstract void Dispose();
    }
}