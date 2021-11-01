using System;
using System.IO;
using System.Text.Json;
using SCVE.Components;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Texts;
using SCVE.Core.UI;
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

            var applicationInit = new ApplicationInit()
            {
                Renderer = new OpenGLRenderer(),
                DeltaTimeProvider = new GlfwDeltaTimeProvider(),
                Window = new GlfwWindow(new WindowProps("Super Cool Video Editor")),
                Input = new GlfwInput(),
                RenderEntitiesCreator = new OpenGLRenderEntitiesCreator(),
                FileLoaders = new PlaygroundFileLoaders(),
                FontAtlasGenerator = new SharpFontImageSharpFontAtlasGenerator(),
            };

            var application = Application.Init(applicationInit);

            application.Renderer.SetFromWindow(application.MainWindow);

            application.ComponentRoot = new ComponentRoot(UIBuilder.Build(File.ReadAllText("assets/UI/simple.ui.xml")));

            application.ComponentRoot.PrintComponentTree(0);
            
            // File.WriteAllText("ui.json", JsonSerializer.Serialize(application.ComponentRoot, new JsonSerializerOptions()
            // {
            //     WriteIndented = true,
            //     IncludeFields = true
            // }));
            
            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}