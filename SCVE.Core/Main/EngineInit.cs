using SCVE.Core.Entities;
using SCVE.Core.Input;
using SCVE.Core.Lifecycle;
using SCVE.Core.Loading.Loaders;
using SCVE.Core.Rendering;
using SCVE.Core.Services;

namespace SCVE.Core.Main
{
    public class EngineInit
    {
        public IRenderer Renderer { get; set; }

        public FileLoaders FileLoaders { get; set; }

        public IDeltaTimeProvider DeltaTimeProvider { get; set; }

        public ScveWindow Window { get; set; }

        public EngineInput EngineInput { get; set; }

        public IRenderEntitiesCreator RenderEntitiesCreator { get; set; }

        public IFontAtlasGenerator FontAtlasGenerator { get; set; }

        public IEngineRunnable Runnable { get; set; }

        public EngineInit(IRenderer renderer, FileLoaders fileLoaders, IDeltaTimeProvider deltaTimeProvider, ScveWindow window, EngineInput engineInput, IRenderEntitiesCreator renderEntitiesCreator, IFontAtlasGenerator fontAtlasGenerator, IEngineRunnable runnable)
        {
            Renderer              = renderer;
            FileLoaders           = fileLoaders;
            DeltaTimeProvider     = deltaTimeProvider;
            Window                = window;
            EngineInput                 = engineInput;
            RenderEntitiesCreator = renderEntitiesCreator;
            FontAtlasGenerator    = fontAtlasGenerator;
            Runnable              = runnable;
        }

        public EngineInit()
        {
        }
    }
}