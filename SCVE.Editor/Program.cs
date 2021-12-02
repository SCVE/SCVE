using SCVE.Engine.Core.Main;
using SCVE.Engine.Default;

namespace SCVE.Editor
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            var engine = ScveEngine.Init(
                new DefaultEngineInit()
                {
                    Runnable = new EditorApp()
                }
            );

            engine.Run();
        }
    }
}