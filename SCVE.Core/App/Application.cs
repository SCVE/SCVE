using System;
using System.Threading;
using SCVE.Core.Entities;
using SCVE.Core.Input;
using SCVE.Core.Rendering;
using SCVE.Core.Services;
using SCVE.Core.Utilities;

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
        public IDeltaTimeProvider DeltaTimeProvider => _scope.DeltaTimeProvider;
        public ScveWindow MainWindow => _scope.MainWindow;
        public InputBase Input => _scope.Input;

        private Application(ApplicationInit init)
        {
            _instance = this;
            _scope = ApplicationScope.FromApplicationInit(init);
        }

        public static Application Init(ApplicationInit init)
        {
            var application = new Application(init);
            
            application._state = AppState.Starting;
            application._scope.Init();
            application._state = AppState.Ready;
            
            _isInited = true;
            
            return application;
        }

        public void Run()
        {
            if (_state != AppState.Ready)
            {
                throw new ScveException("App is not ready");
            }

            _state = AppState.Running;

            float time = 0f;
            Color color = new Color();
            Logger.Warn("Starting Main Loop");
            while (_state == AppState.Running)
            {
                float deltaTime = DeltaTimeProvider.Get();
                _scope.Update(deltaTime);
                
                time += deltaTime;
                float sin = (MathF.Sin(time * MathF.PI) + 1) * 0.5f;
                float brightness = sin * sin;
                color.R = brightness;
                color.G = brightness;
                color.B = brightness;
                color.A = 1;
                
                Renderer.SetClearColor(color);
                Renderer.Clear();

                _scope.Render(Renderer);

                MainWindow.OnUpdate();
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