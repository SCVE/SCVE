using System;
using System.IO;
using SCVE.Core.App;
using SCVE.Default;
using SCVE.UI;
using SCVE.UI.UIBuilders;

namespace UITester
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var application = Application.Init(DefaultApplicationInit.Get());

            var xmlUIBuilder = new XmlUIBuilder(File.ReadAllText("assets/UI/simple.ui.xml"));
            application.Bootstrapable = new BootstrapableComponent(xmlUIBuilder.Build());
            
            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}