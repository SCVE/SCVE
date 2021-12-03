using SCVE.Engine.Core.Input;

namespace SCVE.Engine.Core.Lifecycle
{
    public interface IInputReceiver
    {
        void WindowSizeChanged(int width, int height);
        void KeyDown(KeyCode code);
        void KeyUp(KeyCode code);
        void KeyRepeat(KeyCode code);
        void WindowMaximized();
        void WindowWindowed();
        void WindowMinimized();
        void CursorEnter();
        void CursorLeave();
        void CursorMoved(float x, float y);
        void Scroll(float deltaX, float deltaY);
        void MouseButtonDown(MouseCode code);
        void MouseButtonUp(MouseCode code);
    }
}