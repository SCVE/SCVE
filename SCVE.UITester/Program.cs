using System;
using System.IO;
using SCVE.Core.Main;
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
            
            var application = Engine.Init(new DefaultEngineInit()
            {
                Runnable = new EngineRunnableUI().WithBootstraped(xmlUIBuilder.Build())
            });

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}