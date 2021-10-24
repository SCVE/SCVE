using SCVE.Core.Loading.Loaders;
using SCVE.ImageSharpBindings;
using SCVE.Platform.Windows;

namespace Playground
{
    public class PlaygroundFileLoaders : FileLoaders
    {
        public override IShaderProgramLoader ShaderProgram { get; protected set; } = new WindowsProgramFileLoader();

        public override ITextureLoader Texture { get; protected set; } = new ImageSharpTextureLoader();

        public override IFontLoader Font { get; protected set; } = new WindowsFontFileLoader();
    }
}