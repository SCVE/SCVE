using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.ImageSharpBindings;
using SCVE.OpenTKBindings;

namespace SCVE.Default
{
    public class DefaultApplicationInit : ApplicationInit
    {
        public static ApplicationInit Get()
        {
            return new ApplicationInit()
            {
                Renderer              = new OpenGLRenderer(),
                DeltaTimeProvider     = new GlfwDeltaTimeProvider(),
                Window                = new GlfwWindow(new WindowProps("Super Cool Video Editor")),
                Input                 = new GlfwInput(),
                RenderEntitiesCreator = new OpenGLRenderEntitiesCreator(),
                FileLoaders           = new DefaultFileLoaders(),
                FontAtlasGenerator    = new SharpFontImageSharpFontAtlasGenerator(),
            };
        }
    }
}