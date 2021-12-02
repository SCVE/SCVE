using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.Core.Loading.Loaders
{
    public interface IShaderProgramLoader
    {
        public ShaderProgram Load(string fileName);
    }
}