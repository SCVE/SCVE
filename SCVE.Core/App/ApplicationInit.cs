using SCVE.Core.Entities;
using SCVE.Core.Input;
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
        
        public InputBase Input { get; set; }

        public ApplicationInit(IRenderer renderer, IFileLoader fileLoader, IDeltaTimeProvider deltaTimeProvider, ScveWindow window, InputBase input)
        {
            Renderer = renderer;
            FileLoader = fileLoader;
            DeltaTimeProvider = deltaTimeProvider;
            Window = window;
            Input = input;
        }
    }
}