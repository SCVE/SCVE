using System;
using System.IO;
using SCVE.Components;
using SCVE.Components.Layouts;
using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Caches;
using SCVE.Core.Entities;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.ImageSharpBindings;
using SCVE.Null;
using SCVE.OpenTKBindings;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            if (false)
            {
                var atlasGenerator = new SharpFontImageSharpFontAtlasGenerator();

                atlasGenerator.Generate("arial.ttf", Alphabets.Default, 12);
            }
            
            var applicationInit = new ApplicationInitNull();
            applicationInit.Renderer = new OpenGLRenderer();
            applicationInit.DeltaTimeProvider = new GlfwDeltaTimeProvider();
            applicationInit.Window = new GlfwWindow(new WindowProps("Super Cool Video Editor"));
            applicationInit.Input = new GlfwInput();
            applicationInit.RenderEntitiesCreator = new OpenGLRenderEntitiesCreator();
            applicationInit.FileLoaders = new PlaygroundFileLoaders();
            applicationInit.FontAtlasGenerator = new SharpFontImageSharpFontAtlasGenerator();

            var application = Application.Init(applicationInit);

            application.Renderer.SetFromWindow(application.MainWindow);

            application.RootComponent = UIBuilder.Build(File.ReadAllText("assets/UI/default.ui.xml"));

            application.RootComponent.SetPositionAndSize(0, 0, application.MainWindow.Width, application.MainWindow.Height);

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}