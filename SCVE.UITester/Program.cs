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
            var xmlUIBuilder = new XmlUIBuilder(File.ReadAllText("assets/UI/editor.ui.xml"));
            
            var application = Application.Init(new DefaultApplicationInit()
            {
                Bootstrapable = new BootstrapableUI().WithBootstraped(xmlUIBuilder.Build())
            });

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}