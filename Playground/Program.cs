using System;
using SCVE.Components;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Primitives;
using SCVE.Core.Services;
using SCVE.ImageSharpBindings;
using SCVE.Null;
using SCVE.OpenTKBindings;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationInit = new ApplicationInitNull();
            applicationInit.Renderer = new OpenGLRenderer();
            applicationInit.DeltaTimeProvider = new GlfwDeltaTimeProvider();
            applicationInit.Window = new GlfwWindow(new WindowProps("Super Cool Video Editor"));
            applicationInit.Input = new GlfwInput();
            applicationInit.RenderEntitiesCreator = new OpenGLRenderEntitiesCreator();
            applicationInit.FileLoader = new FileLoader();
            applicationInit.TextureLoader = new ImageSharpTextureLoader();

            var application = Application.Init(applicationInit);

            application.ViewProjectionAccessor.SetFromWindow();

            // application.ViewProjectionAccessor.SetView(ScveMatrix4X4.Identity.Set(2, 3, -1));

            var rootComponent = new EmptyComponent(new Rect(0, 0, 2, 2));
            // application.RootComponent = rootComponent;
            application.RootComponent = new FullFlexComponent(new Rect(0, 0, 600, 100));

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}