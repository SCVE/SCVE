using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public class Camera
    {
        public ref Matrix4 GetProjection()
        {
            return ref _projection;
        }
        
        protected Matrix4 _projection = Matrix4.Identity;
    }
}