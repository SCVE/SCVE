using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Engine.Core.Services;

namespace SCVE.Engine.OpenTKBindings
{
    public class GlfwDeltaTimeProvider : IDeltaTimeProvider
    {
        private float _lastTime;
        
        public float Get()
        {
            var currentTime = (float)GLFW.GetTime();
            float deltaTime = currentTime - _lastTime;
            _lastTime = currentTime;
            return deltaTime;
        }
    }
}