using System;
using System.Threading;
using SCVE.Core.Entities;
using SCVE.Core.Input;
using SCVE.Core.Primitives;
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
        public IRenderEntitiesCreator RenderEntitiesCreator => _scope.RenderEntitiesCreator;

        public ITextureLoader TextureLoader => _scope.TextureLoader;

        public Component RootComponent { get; set; }

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

            if (RootComponent is null)
            {
                throw new ScveException("No root component is present");
            }

            _state = AppState.Running;

            float time = 0f;
            ColorRgba colorRgba = new ColorRgba();
            Logger.Warn("Starting Main Loop");
            while (_state == AppState.Running)
            {
                float deltaTime = DeltaTimeProvider.Get();
                _scope.Update(deltaTime);

                // time += deltaTime;
                // float sin = (MathF.Sin(time * MathF.PI) + 1) * 0.5f;
                // float brightness = sin * sin;
                // color.R = brightness;
                // color.G = brightness;
                // color.B = brightness;
                // color.A = 1;

                // Renderer.SetClearColor(color);

                Renderer.Clear();

                RootComponent.Render(Renderer);

                MainWindow.SetTitle($"FPS: {1 / deltaTime}");

                MainWindow.OnUpdate();
            }

            if (_state != AppState.TerminationRequested)
            {
                throw new ScveException("Invalid state was set, application terminated abnormally");
            }

            _state = AppState.Terminating;
        }

        public void RunOnce()
        {
            if (_state != AppState.Ready)
            {
                throw new ScveException("App is not ready");
            }

            if (RootComponent is null)
            {
                throw new ScveException("No root component is present");
            }

            _state = AppState.Running;

            float time = 0f;
            ColorRgba colorRgba = new ColorRgba();
            Logger.Warn("Starting Main Loop");

            float deltaTime = DeltaTimeProvider.Get();
            _scope.Update(deltaTime);

            // time += deltaTime;
            // float sin = (MathF.Sin(time * MathF.PI) + 1) * 0.5f;
            // float brightness = sin * sin;
            // color.R = brightness;
            // color.G = brightness;
            // color.B = brightness;
            // color.A = 1;

            // Renderer.SetClearColor(color);

            Renderer.Clear();

            RootComponent.Render(Renderer);

            MainWindow.SetTitle($"FPS: {1 / deltaTime}");

            MainWindow.OnUpdate();

            _state = AppState.TerminationRequested;

            if (_state != AppState.TerminationRequested)
            {
                throw new ScveException("Invalid state was set, application terminated abnormally");
            }

            _state = AppState.Terminating;
        }

        public void RequestTerminate()
        {
            Console.WriteLine($"Requesting app termination; Thread {Thread.CurrentThread.ManagedThreadId}");
            _state = AppState.TerminationRequested;
        }
    }
}