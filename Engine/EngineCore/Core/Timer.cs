using System.Diagnostics;

namespace Engine.EngineCore.Core
{
    public class Timer
    {
        public Timer()
        {
            Reset();
        }

        public void Reset()
        {
            _stopwatch = Stopwatch.StartNew();
        }
        
        public float Elapsed()
        {
            return _stopwatch.ElapsedMilliseconds / 1000f;
        }
        
        public float ElapsedMillis()
        {
            return _stopwatch.ElapsedMilliseconds;
        }

        private Stopwatch _stopwatch;
    }
}