namespace Engine.EngineCore.Core
{
    public struct ApplicationCommandLineArgs
    {
        public int Count;
        public string[] Args;

        public string this[int index]
        {
            get => Args[index];
        }
    }
}