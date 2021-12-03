using System.Collections.Generic;
using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Lifecycle;
using SCVE.Engine.Core.Loading.Loaders;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Services;

namespace SCVE.Engine.Core.Main
{
    public class EngineScope
    {
        public IRenderer Renderer { get; private set; }
        public FileLoaders FileLoaders { get; private set; }
        public IDeltaTimeProvider DeltaTimeProvider { get; private set; }

        private List<IInitable> _initables;
        private List<IUpdatable> _updatables;
        private List<ITerminatable> _terminatables;
        
        public ScveWindow MainWindow => ScveWindow.Instance;
        
        public EngineInput EngineInput { get; private set; }
        
        public IRenderEntitiesCreator RenderEntitiesCreator { get; private set; }

        public EngineScope()
        {
            _initables = new List<IInitable>();
            _updatables = new List<IUpdatable>();
            _terminatables = new List<ITerminatable>();
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
                terminatable.OnTerminate();
            }
        }

        public EngineScope WithRenderer(IRenderer renderer)
        {
            if (Renderer is not null)
            {
                EndTrack(Renderer);
            }

            Renderer = renderer;
            BeginTrack(renderer);
            return this;
        }

        public EngineScope WithFileLoaders(FileLoaders fileLoaders)
        {
            if (FileLoaders is not null)
            {
                EndTrack(FileLoaders);
            }

            FileLoaders = fileLoaders;
            BeginTrack(fileLoaders);
            return this;
        }

        public EngineScope WithDeltaTimeProvider(IDeltaTimeProvider deltaTimeProvider)
        {
            if (DeltaTimeProvider is not null)
            {
                EndTrack(DeltaTimeProvider);
            }
            DeltaTimeProvider = deltaTimeProvider;
            BeginTrack(deltaTimeProvider);
            return this;
        }

        public EngineScope WithInput(EngineInput engineInput)
        {
            EngineInput = engineInput;
            return this;
        }

        public EngineScope WithRenderEntitiesProvider(IRenderEntitiesCreator renderEntitiesCreator)
        {
            RenderEntitiesCreator = renderEntitiesCreator;
            return this;
        }

        public static EngineScope FromEngineInit(EngineInit engineInit)
        {
            return new EngineScope()
                .WithRenderer(engineInit.Renderer)
                .WithFileLoaders(engineInit.FileLoaders)
                .WithDeltaTimeProvider(engineInit.DeltaTimeProvider)
                .WithInput(engineInit.EngineInput)
                .WithRenderEntitiesProvider(engineInit.RenderEntitiesCreator);
        }
    }
}