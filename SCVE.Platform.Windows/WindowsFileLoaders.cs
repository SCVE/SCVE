using SCVE.Core.Rendering;
using SCVE.Core.Services;
using SCVE.Core.Texts;

namespace SCVE.Platform.Windows
{
    public class WindowsFileLoaders : FileLoaders
    {
        public override FileLoader<ShaderProgram> Program { get; protected set; } = new WindowsProgramFileLoader();

        public override FileLoader<FontAtlasData> FontAtlasData { get; protected set; } = new WindowsFontAtlasDataFileLoader();
    }
}