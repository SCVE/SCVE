using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Lifecycle;
using SCVE.Engine.Core.Loading.Loaders;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Services;

namespace SCVE.Engine.Core.Main
{
    public class EngineInit
    {
        public IRenderer Renderer { get; set; }

        public FileLoaders FileLoaders { get; set; }

        public IDeltaTimeProvider DeltaTimeProvider { get; set; }

        public ScveWindow Window { get; set; }

        public EngineInput EngineInput { get; set; }

        public IRenderEntitiesCreator RenderEntitiesCreator { get; set; }

        public IEngineRunnable Runnable { get; set; }

        public EngineInit(IRenderer renderer, FileLoaders fileLoaders, IDeltaTimeProvider deltaTimeProvider, ScveWindow window, EngineInput engineInput, IRenderEntitiesCreator renderEntitiesCreator, IEngineRunnable runnable)
        {
            Renderer              = renderer;
            FileLoaders           = fileLoaders;
            DeltaTimeProvider     = deltaTimeProvider;
            Window                = window;
            EngineInput           = engineInput;
            RenderEntitiesCreator = renderEntitiesCreator;
            Runnable              = runnable;
        }

        public EngineInit()
        {
        }
    }
}