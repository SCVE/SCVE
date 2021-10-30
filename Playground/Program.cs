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

            application.ComponentRoot = new ComponentRoot(UIBuilder.Build(File.ReadAllText("assets/UI/default.ui.xml")));

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