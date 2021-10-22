using SCVE.Core.Entities;
using SCVE.Core.Input;
using SCVE.Core.Rendering;
using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class ApplicationInit
    {
        public IRenderer Renderer { get; set; }

        public FileLoaders FileLoaders { get; set; }

        public IDeltaTimeProvider DeltaTimeProvider { get; set; }

        public ScveWindow Window { get; set; }

        public InputBase Input { get; set; }

        public IRenderEntitiesCreator RenderEntitiesCreator { get; set; }

        public ITextureLoader TextureLoader { get; set; }

        public IFontAtlasGenerator FontAtlasGenerator { get; set; }

        public ApplicationInit(IRenderer renderer, FileLoaders fileLoaders, IDeltaTimeProvider deltaTimeProvider, ScveWindow window, InputBase input, IRenderEntitiesCreator renderEntitiesCreator, ITextureLoader textureLoader, IFontAtlasGenerator fontAtlasGenerator)
        {
            Renderer = renderer;
            FileLoaders = fileLoaders;
            DeltaTimeProvider = deltaTimeProvider;
            Window = window;
            Input = input;
            RenderEntitiesCreator = renderEntitiesCreator;
            TextureLoader = textureLoader;
            FontAtlasGenerator = fontAtlasGenerator;
        }

        protected ApplicationInit()
        {
        }
    }
}