using System;
using SCVE.Components;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Primitives;
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

            var rootComponent = new EmptyComponent(new Rect(0, 0, 2, 2));
            application.RootComponent = rootComponent;

            rootComponent.AddChild(new RectComponent(
                new Rect(-0.5f, 0.5f, 0.8f, 0.8f),
                new ColorRgba(1, 0, 0, 1)
            ));
            rootComponent.AddChild(new RectComponent(
                new Rect(0.5f, 0.5f, 0.8f, 0.8f),
                new ColorRgba(0, 1, 0, 1)
            ));
            rootComponent.AddChild(new RectComponent(
                new Rect(-0.5f, -0.5f, 0.8f, 0.8f),
                new ColorRgba(0, 0, 1, 1)
            ));
            rootComponent.AddChild(new RectComponent(
                new Rect(0.5f, -0.5f, 0.8f, 0.8f),
                new ColorRgba(1, 1, 1, 1)
            ));

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}