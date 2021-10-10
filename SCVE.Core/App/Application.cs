using SCVE.Core.Entities;
using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class Application
    {
        private static bool _isInited;
        
        private static Application _instance;

        public static Application Instance => _instance;

        private bool _terminate;

        private ApplicationScope _scope;

        public IRenderer Renderer => _scope.Renderer;
        public IFileLoader FileLoader => _scope.FileLoader;
        public WindowManager WindowManager => _scope.WindowManager;
        public IDeltaTimeProvider DeltaTimeProvider => _scope.DeltaTimeProvider;
        
        public ScveWindow MainWindow => WindowManager.MainWindow;

        private Application(ApplicationInit init)
        {
            _instance = this;
            _scope = ApplicationScope.FromApplicationInit(init);
        }

        public static Application Init(ApplicationInit init)
        {
            var application = new Application(init);
            _isInited = true;
            return application;
        }

        public void Init()
        {
            _scope.Init();
        }

        public void Run()
        {
            while (!_terminate)
            {
                WindowManager.PollEvents();
                float deltaTime = DeltaTimeProvider.Get();
                _scope.Update(deltaTime);
                
                _scope.Render(Renderer);
            }
        }

        public void AddRenderable(IRenderable renderable)
        {
            _scope.Renderables.Add(renderable);
        }

        public void Terminate()
        {
            _terminate = true;
            _scope.Terminate();
        }
    }
}