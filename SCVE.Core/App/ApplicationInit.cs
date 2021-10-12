using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class ApplicationInit
    {
        public IRenderer Renderer { get; set; }

        public IFileLoader FileLoader { get; set; }

        public IDeltaTimeProvider DeltaTimeProvider { get; set; }

        public ScveWindow Window { get; set; }

        public ApplicationInit(IRenderer renderer, IFileLoader fileLoader, IDeltaTimeProvider deltaTimeProvider, ScveWindow window)
        {
            Renderer = renderer;
            FileLoader = fileLoader;
            DeltaTimeProvider = deltaTimeProvider;
            Window = window;
        }
    }
}