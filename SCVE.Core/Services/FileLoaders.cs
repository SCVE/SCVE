using SCVE.Core.Loading;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;

namespace SCVE.Core.Services
{
    public abstract class FileLoaders
    {
        public abstract FileLoader<ShaderProgram> ShaderProgram { get; protected set; }
        
        public abstract FileLoader<FontAtlasData> FontAtlasData { get; protected set; }
    }
}