using System;
using System.IO;
using SCVE.Core.App;
using SCVE.Default;
using SCVE.UI;
using SCVE.UI.UIBuilders;

namespace SCVE.UITreeDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            var xmlUIBuilder = new XmlUIBuilder(File.ReadAllText("assets/UI/editor.ui.xml"));
            var analyzeUI = new BootstrapableUI().WithBootstraped(xmlUIBuilder.Build());

            var application = Application.Init(new DefaultApplicationInit()
            {
                Bootstrapable = new BootstrapableTreeViewer(analyzeUI)
            });

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}