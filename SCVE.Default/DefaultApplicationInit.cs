using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.ImageSharpBindings;
using SCVE.OpenTKBindings;

namespace SCVE.Default
{
    public class DefaultApplicationInit : ApplicationInit
    {
        public DefaultApplicationInit()
        {
            Renderer              = new OpenGLRenderer();
            DeltaTimeProvider     = new GlfwDeltaTimeProvider();
            Window                = new GlfwWindow(new WindowProps("Super Cool Video Editor", 1600, 900));
            Input                 = new GlfwInput();
            RenderEntitiesCreator = new OpenGLRenderEntitiesCreator();
            FileLoaders           = new DefaultFileLoaders();
            FontAtlasGenerator    = new SharpFontImageSharpFontAtlasGenerator();
            Bootstrapable         = null;
        }
    }
}