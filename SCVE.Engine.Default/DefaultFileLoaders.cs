using SCVE.Engine.Core.Loading.Loaders;
using SCVE.Engine.ImageSharpBindings;
using SCVE.Engine.Platform.Windows;

namespace SCVE.Engine.Default
{
    public class DefaultFileLoaders : FileLoaders
    {
        public override IShaderProgramLoader ShaderProgram { get; protected set; } = new WindowsProgramFileLoader();

        public override ITextureLoader Texture { get; protected set; } = new ImageSharpTextureLoader();
    }
}