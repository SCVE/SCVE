using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Main;
using SCVE.Engine.OpenTKBindings;

namespace SCVE.Engine.Default
{
    public class DefaultEngineInit : EngineInit
    {
        public DefaultEngineInit()
        {
            Renderer              = new OpenGLRenderer();
            DeltaTimeProvider     = new GlfwDeltaTimeProvider();
            Window                = new GlfwWindow(new WindowProps("Super Cool Video Editor", 1600, 900));
            EngineInput           = new GlfwEngineInput();
            RenderEntitiesCreator = new OpenGLRenderEntitiesCreator();
            FileLoaders           = new DefaultFileLoaders();
            Runnable              = null;
        }
    }
}