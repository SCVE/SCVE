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
            var xmlUIBuilder = new XmlUIBuilder();
            var component = xmlUIBuilder.Build("assets/UI/editor.ui.xml");
            var analyzeUI = new EngineRunnableUI().WithBootstraped(component);

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