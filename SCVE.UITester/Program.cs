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
            var xmlUIBuilder = new XmlUIBuilder();
            var component = xmlUIBuilder.Build("assets/UI/editor.ui.xml");
            var application = Engine.Init(new DefaultEngineInit()
            {
                Runnable = new EngineRunnableUI().WithBootstraped(component)
            });

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }
    }
}