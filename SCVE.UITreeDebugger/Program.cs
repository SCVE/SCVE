using System;
using System.IO;
using SCVE.Core.Main;
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
            var analyzeUI = new EngineRunnableUI().WithBootstraped(xmlUIBuilder.Build());

            var application = Engine.Init(new DefaultEngineInit()
            {
                Runnable = new EngineRunnableTreeViewer(analyzeUI)
            });

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}