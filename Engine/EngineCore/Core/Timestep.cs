namespace Engine.EngineCore.Core
{
    public class Timestep
    {
        public Timestep(float time)
        {
            _time = time;
        }

        public static implicit operator float(Timestep timestep)
        {
            return timestep._time;
        }

        public float GetSeconds()
        {
            return _time;
        }
        
        public float GetMilliseconds()
        {
            return _time * 1000f;
        }

        private float _time;
    }
}