using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class ApplicationInit
    {
        public IRenderer Renderer { get; set; }

        public IFileLoader FileLoader { get; set; }
        
        public WindowManager WindowManager { get; set; }
        
        public IDeltaTimeProvider DeltaTimeProvider { get; set; }

        public ApplicationInit(IRenderer renderer, IFileLoader fileLoader, WindowManager windowManager, IDeltaTimeProvider deltaTimeProvider)
        {
            Renderer = renderer;
            FileLoader = fileLoader;
            WindowManager = windowManager;
            DeltaTimeProvider = deltaTimeProvider;
        }
    }
}