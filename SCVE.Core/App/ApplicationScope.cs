using System.Collections.Generic;
using SCVE.Core.Entities;
using SCVE.Core.Input;
using SCVE.Core.Lifecycle;
using SCVE.Core.Loading.Loaders;
using SCVE.Core.Rendering;
using SCVE.Core.Services;

namespace SCVE.Core.App
{
    public class ApplicationScope
    {
        public IRenderer Renderer { get; private set; }
        public FileLoaders FileLoaders { get; private set; }
        public IDeltaTimeProvider DeltaTimeProvider { get; private set; }

        private List<IInitable> _initables;
        private List<IUpdatable> _updatables;
        private List<ITerminatable> _terminatables;
        
        public ScveWindow MainWindow => ScveWindow.Instance;
        
        public InputBase Input { get; private set; }
        
        public IRenderEntitiesCreator RenderEntitiesCreator { get; private set; }
        public IFontAtlasGenerator FontAtlasGenerator { get; set; }

        public ApplicationScope()
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

        public ApplicationScope WithFileLoaders(FileLoaders fileLoaders)
        {
            if (FileLoaders is not null)
            {
                EndTrack(FileLoaders);
            }

            FileLoaders = fileLoaders;
            BeginTrack(fileLoaders);
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

        public ApplicationScope WithInput(InputBase input)
        {
            Input = input;
            return this;
        }

        public ApplicationScope WithRenderEntitiesProvider(IRenderEntitiesCreator renderEntitiesCreator)
        {
            RenderEntitiesCreator = renderEntitiesCreator;
            return this;
        }
        
        public ApplicationScope WithFontAtlasGenerator(IFontAtlasGenerator fontAtlasGenerator)
        {
            FontAtlasGenerator = fontAtlasGenerator;
            return this;
        }

        public static ApplicationScope FromApplicationInit(ApplicationInit applicationInit)
        {
            return new ApplicationScope()
                .WithRenderer(applicationInit.Renderer)
                .WithFileLoaders(applicationInit.FileLoaders)
                .WithDeltaTimeProvider(applicationInit.DeltaTimeProvider)
                .WithInput(applicationInit.Input)
                .WithRenderEntitiesProvider(applicationInit.RenderEntitiesCreator)
                .WithFontAtlasGenerator(applicationInit.FontAtlasGenerator);
        }
    }
}