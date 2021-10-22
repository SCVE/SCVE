using SCVE.Core.Rendering;
using SCVE.Core.Texts;

namespace SCVE.Core.Services
{
    public abstract class FileLoaders
    {
        public abstract FileLoader<ShaderProgram> Program { get; protected set; }
        
        public abstract FileLoader<FontAtlasData> FontAtlasData { get; protected set; }
    }
}