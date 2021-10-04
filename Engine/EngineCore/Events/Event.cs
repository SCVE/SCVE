using System;

namespace Engine.EngineCore.Events
{
    public enum EventType
    {
        None = 0,
        WindowClose, WindowResize, WindowFocus, WindowLostFocus, WindowMoved,
        AppTick, AppUpdate, AppRender, KeyPressed,
        KeyReleased, KeyTyped,
        MouseButtonPressed, MouseButtonReleased, MouseMoved, MouseScrolled,
    }

    [Flags]
    public enum EventCategory : int
    {
        None = 0,
        EventCategoryApplication = 1 << 0,
        EventCategoryInput = 1 << 1,
        EventCategoryKeyboard = 1 << 2,
        EventCategoryMouse = 1 << 3,
        EventCategoryMouseButton = 1 << 4
    }

    public class Event
    {
        public bool Handled;

        public virtual EventType GetEventType()
        {
            return EventType.None;
        }

        public virtual string GetName()
        {
            return "";
        }

        public virtual int GetCategoryFlags()
        {
            return 0;
        }

        public override string ToString()
        {
            return GetName();
        }

        public bool IsInCategory(EventCategory category)
        {
            return (GetCategoryFlags() & (int)category) != 0;
        }
    }

    public class EventDispatcher
    {
        public EventDispatcher(Event @event)
        {
            _event = @event;
        }

        public bool Dispatch<T>(Func<T, bool> func)
            where T : Event
        {
            if (_event.GetEventType().GetType() == typeof(T))
            {
                _event.Handled |= func((T)_event);
                return true;
            }

            return false;
        }

        private Event _event;
    }
}