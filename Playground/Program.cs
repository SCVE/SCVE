using System;
using System.Threading.Tasks;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Null;
using SCVE.OpenTK.Glfw;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationInit = new ApplicationInitNull();
            applicationInit.WindowManager = new GlfwWindowManager();
            var application = Application.Init(applicationInit);

            application.Init();

            application.WindowManager.Create(new WindowProps("SCVE1", isMain: true));
            // application.WindowManager.Create(new WindowProps("SCVE2"));

            application.AddRenderable(new RenderableNull());

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}