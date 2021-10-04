using OpenTK.Mathematics;

namespace Engine.EngineCore.Core
{
    public abstract class Input
    {
        public static Input Instance;
        
        public abstract bool IsKeyPressed(KeyCode key);
        public abstract bool IsMouseButtonPressed(MouseCode button);
        public abstract Vector2 GetMousePosition();
        public abstract float GetMouseX();
        public abstract float GetMouseY();
    }
}