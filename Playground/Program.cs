using System;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Null;
using SCVE.OpenTKBindings;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationInit = new ApplicationInitNull();
            applicationInit.WindowManager = new GlfwWindowManager();
            applicationInit.Renderer = new OpenGLRenderer();
            var application = Application.Init(applicationInit);

            application.Init();

            application.WindowManager.Create(new WindowProps("SCVE1", isMain: true));
            // application.WindowManager.Create(new WindowProps("SCVE2"));

            application.AddRenderable(new RenderableNull());

            application.DeferedInit();
            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}