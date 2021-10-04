namespace Engine.EngineCore.Core
{
    public class EntryPoint
    {
        static extern Application CreateApplication(ApplicationCommandLineArgs args);
        
        public static void Main(string[] args)
        {
            var app = CreateApplication(new ApplicationCommandLineArgs()
            {
                Args = args,
                Count = args.Length
            });

            app.Run();
        }
    }
}