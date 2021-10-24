using SCVE.Core.Rendering;

namespace SCVE.Core.Loading.Loaders
{
    public interface IShaderProgramLoader
    {
        public ShaderProgram Load(string fileName);
    }
}