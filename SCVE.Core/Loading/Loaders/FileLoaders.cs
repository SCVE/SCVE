namespace SCVE.Core.Loading.Loaders
{
    public abstract class FileLoaders
    {
        public abstract IShaderProgramLoader ShaderProgram { get; protected set; }
        
        public abstract ITextureLoader Texture { get; protected set; }
    }
}