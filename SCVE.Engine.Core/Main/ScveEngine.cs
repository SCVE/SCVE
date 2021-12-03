using System;
using System.Threading;
using SCVE.Engine.Core.Caches;
using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Lifecycle;
using SCVE.Engine.Core.Loading.Loaders;
using SCVE.Engine.Core.Misc;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Services;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.Core.Main
{
    public class ScveEngine
    {
        private static ScveEngine _instance;

        public static ScveEngine Instance => _instance;

        private EngineState _state;

        private EngineScope _scope;

        public IRenderer Renderer => _scope.Renderer;
        public FileLoaders FileLoaders => _scope.FileLoaders;
        public IDeltaTimeProvider DeltaTimeProvider => _scope.DeltaTimeProvider;
        public ScveWindow MainWindow => _scope.MainWindow;
        public EngineInput Input => _scope.EngineInput;
        public IRenderEntitiesCreator RenderEntitiesCreator => _scope.RenderEntitiesCreator;

        public CachesContainer Cache;

        public IEngineRunnable Runnable { get; set; }

        private ScveEngine(EngineInit init)
        {
            _instance = this;
            _scope    = EngineScope.FromEngineInit(init);
            Runnable  = init.Runnable;
        }

        public static ScveEngine Init(EngineInit init)
        {
            var application = new ScveEngine(init);

            application.Cache = new();

            application._state = EngineState.Starting;
            application._scope.Init();
            application._state = EngineState.Ready;

            application.Renderer.SetFromWindow(application.MainWindow);

            application.Runnable.Init();

            return application;
        }

        public void Run()
        {
            if (_state != EngineState.Ready)
            {
                throw new ScveException("App is not ready");
            }

            if (Runnable is null)
            {
                throw new ScveException("No component root is present");
            }

            _state = EngineState.Running;

            Logger.Warn("Starting Main Loop");
            while (_state == EngineState.Running)
            {
                float deltaTime = DeltaTimeProvider.Get();
                _scope.Update(deltaTime);

                Renderer.Clear();

                Runnable.Update(deltaTime);
                Runnable.Render(Renderer);

                MainWindow.OnUpdate();
            }

            if (_state != EngineState.TerminationRequested)
            {
                throw new ScveException("Invalid state was set, application terminated abnormally");
            }

            _state = EngineState.Terminating;
        }

        public void RunOnce()
        {
            if (_state != EngineState.Ready)
            {
                throw new ScveException("App is not ready");
            }

            if (Runnable is null)
            {
                throw new ScveException("No root component is present");
            }

            _state = EngineState.Running;

            Logger.Warn("Starting Main Loop");

            float deltaTime = DeltaTimeProvider.Get();
            _scope.Update(deltaTime);

            Renderer.Clear();

            Runnable.Update(deltaTime);

            Runnable.Render(Renderer);

            MainWindow.OnUpdate();

            _state = EngineState.TerminationRequested;

            if (_state != EngineState.TerminationRequested)
            {
                throw new ScveException("Invalid state was set, application terminated abnormally");
            }

            _state = EngineState.Terminating;
        }

        public void RequestTerminate()
        {
            Console.WriteLine($"Requesting app termination; Thread {Thread.CurrentThread.ManagedThreadId}");
            _state = EngineState.TerminationRequested;
        }
    }
}