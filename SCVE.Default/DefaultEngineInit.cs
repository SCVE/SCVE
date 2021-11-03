using SCVE.Core.Entities;
using SCVE.Core.Main;
using SCVE.ImageSharpBindings;
using SCVE.OpenTKBindings;

namespace SCVE.Default
{
    public class DefaultEngineInit : EngineInit
    {
        public DefaultEngineInit()
        {
            Renderer              = new OpenGLRenderer();
            DeltaTimeProvider     = new GlfwDeltaTimeProvider();
            Window                = new GlfwWindow(new WindowProps("Super Cool Video Editor", 1600, 900));
            Input                 = new GlfwInput();
            RenderEntitiesCreator = new OpenGLRenderEntitiesCreator();
            FileLoaders           = new DefaultFileLoaders();
            FontAtlasGenerator    = new SharpFontImageSharpFontAtlasGenerator();
            Runnable         = null;
        }
    }
}