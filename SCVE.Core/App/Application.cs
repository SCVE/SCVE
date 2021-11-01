﻿using System;
using System.Threading;
using SCVE.Core.Caches;
using SCVE.Core.Entities;
using SCVE.Core.Input;
using SCVE.Core.Lifecycle;
using SCVE.Core.Loading.Loaders;
using SCVE.Core.Misc;
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
        public FileLoaders FileLoaders => _scope.FileLoaders;
        public IDeltaTimeProvider DeltaTimeProvider => _scope.DeltaTimeProvider;
        public ScveWindow MainWindow => _scope.MainWindow;
        public InputBase Input => _scope.Input;
        public IRenderEntitiesCreator RenderEntitiesCreator => _scope.RenderEntitiesCreator;

        public IFontAtlasGenerator FontAtlasGenerator => _scope.FontAtlasGenerator;

        public CachesContainer Cache;

        public IBootstrapable Bootstrapable { get; set; }

        private Application(ApplicationInit init)
        {
            _instance     = this;
            _scope        = ApplicationScope.FromApplicationInit(init);
            Bootstrapable = init.Bootstrapable;
        }

        public static Application Init(ApplicationInit init)
        {
            var application = new Application(init);

            application.Cache = new();

            application._state = AppState.Starting;
            application._scope.Init();
            application._state = AppState.Ready;

            application.Renderer.SetFromWindow(application.MainWindow);
            
            application.Bootstrapable.Init();

            _isInited = true;

            return application;
        }

        public void Run()
        {
            if (_state != AppState.Ready)
            {
                throw new ScveException("App is not ready");
            }

            if (Bootstrapable is null)
            {
                throw new ScveException("No component root is present");
            }

            _state = AppState.Running;

            Logger.Warn("Starting Main Loop");
            while (_state == AppState.Running)
            {
                float deltaTime = DeltaTimeProvider.Get();
                _scope.Update(deltaTime);

                Renderer.Clear();

                Bootstrapable.Update(deltaTime);
                Bootstrapable.Render(Renderer);

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

            if (Bootstrapable is null)
            {
                throw new ScveException("No root component is present");
            }

            _state = AppState.Running;

            Logger.Warn("Starting Main Loop");

            float deltaTime = DeltaTimeProvider.Get();
            _scope.Update(deltaTime);

            Renderer.Clear();
            
            Bootstrapable.Update(deltaTime);

            Bootstrapable.Render(Renderer);

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