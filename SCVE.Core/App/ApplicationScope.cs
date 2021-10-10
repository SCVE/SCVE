using System.Collections.Generic;
using SCVE.Core.Entities;
using SCVE.Core.Lifecycle;
using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class ApplicationScope
    {
        public IRenderer Renderer { get; private set; }
        public IFileLoader FileLoader { get; private set; }
        public WindowManager WindowManager { get; private set; }
        public IDeltaTimeProvider DeltaTimeProvider { get; private set; }

        private List<IInitable> _initables;
        private List<IUpdatable> _updatables;
        private List<ITerminatable> _terminatables;
        
        public List<IRenderable> Renderables { get; private set; }

        public ApplicationScope()
        {
            _initables = new List<IInitable>();
            _updatables = new List<IUpdatable>();
            _terminatables = new List<ITerminatable>();
            Renderables = new List<IRenderable>();
        }
        
        private void BeginTrack(object entity)
        {
            if (entity is IInitable initable)
            {
                _initables.Add(initable);
            }

            if (entity is ITerminatable terminatable)
            {
                _terminatables.Add(terminatable);
            }

            if (entity is IUpdatable updatable)
            {
                _updatables.Add(updatable);
            }
        }

        private void EndTrack(object entity)
        {
            if (entity is IInitable initable)
            {
                _initables.Remove(initable);
            }

            if (entity is ITerminatable terminatable)
            {
                _terminatables.Remove(terminatable);
            }
        }

        public void Init()
        {
            foreach (var initable in _initables)
            {
                initable.Init();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var updatable in _updatables)
            {
                updatable.Update(deltaTime);
            }
        }

        public void Terminate()
        {
            foreach (var terminatable in _terminatables)
            {
                terminatable.Terminate();
            }
        }

        public void Render(IRenderer renderer)
        {
            foreach (var renderable in Renderables)
            {
                renderable.Render(renderer);
            }
        }

        public ApplicationScope WithRenderer(IRenderer renderer)
        {
            if (Renderer is not null)
            {
                EndTrack(Renderer);
            }

            Renderer = renderer;
            BeginTrack(renderer);
            return this;
        }

        public ApplicationScope WithFileStorage(IFileLoader fileLoader)
        {
            if (FileLoader is not null)
            {
                EndTrack(FileLoader);
            }

            FileLoader = fileLoader;
            BeginTrack(fileLoader);
            return this;
        }

        public ApplicationScope WithWindowManager(WindowManager windowManager)
        {
            if (WindowManager is not null)
            {
                EndTrack(WindowManager);
            }
            WindowManager = windowManager;
            BeginTrack(windowManager);
            return this;
        }

        public ApplicationScope WithDeltaTimeProvider(IDeltaTimeProvider deltaTimeProvider)
        {
            if (DeltaTimeProvider is not null)
            {
                EndTrack(DeltaTimeProvider);
            }
            DeltaTimeProvider = deltaTimeProvider;
            BeginTrack(deltaTimeProvider);
            return this;
        }

        public static ApplicationScope FromApplicationInit(ApplicationInit applicationInit)
        {
            return new ApplicationScope()
                .WithRenderer(applicationInit.Renderer)
                .WithFileStorage(applicationInit.FileLoader)
                .WithWindowManager(applicationInit.WindowManager)
                .WithDeltaTimeProvider(applicationInit.DeltaTimeProvider);
        }
    }
}