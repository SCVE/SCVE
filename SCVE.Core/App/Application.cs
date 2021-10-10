using System;
using System.Threading;
using SCVE.Core.Entities;
using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class Application
    {
        private static bool _isInited;

        private static Application _instance;

        public static Application Instance => _instance;

        private AppState _state;

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
            _state = AppState.Starting;
            _scope.Init();
            _state = AppState.Ready;
        }

        public void Run()
        {
            if (_state != AppState.Ready)
            {
                throw new ScveException("App is not ready");
            }
            _state = AppState.Running;
            
            while (_state == AppState.Running)
            {
                float deltaTime = DeltaTimeProvider.Get();
                _scope.Update(deltaTime);

                _scope.Render(Renderer);
                WindowManager.PollEvents();
            }

            if (_state != AppState.TerminationRequested)
            {
                throw new ScveException("Invalid state was set, application terminated abnormally");
            }

            _state = AppState.Terminating;
        }

        public void AddRenderable(IRenderable renderable)
        {
            _scope.Renderables.Add(renderable);
        }

        public void RequestTerminate()
        {
            Console.WriteLine($"Requesting app termination; Thread {Thread.CurrentThread.ManagedThreadId}");
            _state = AppState.TerminationRequested;
        }
    }
}