using System;
using SCVE.Components;
using SCVE.Core.App;
using SCVE.Core.Entities;
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
            applicationInit.TextureLoader = new ImageSharpTextureLoader();

            var application = Application.Init(applicationInit);

            application.RootComponent = new BasicComponent();

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}