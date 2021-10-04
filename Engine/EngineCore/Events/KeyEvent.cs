using Engine.EngineCore.Core;

namespace Engine.EngineCore.Events
{
    public class KeyEvent : Event
    {
        public KeyCode GetKeyCode()
        {
            return _keyCode;
        }

        public override int GetCategoryFlags()
        {
            return (int)(EventCategory.EventCategoryKeyboard | EventCategory.EventCategoryInput);
        }

        protected KeyEvent(KeyCode keyCode)
        {
            _keyCode = keyCode;
        }

        protected KeyCode _keyCode;
    }

    public class KeyPressedEvent : KeyEvent
    {
        public KeyPressedEvent(KeyCode keyCode, ushort repeatCount) : base(keyCode)
        {
            _repeatCount = repeatCount;
        }

        public ushort GetRepeatCount()
        {
            return _repeatCount;
        }

        public override string ToString()
        {
            return "KeyPressedEvent: " + _keyCode + " (" + _repeatCount + " repeats)";
        }

        public static EventType GetStaticType()
        {
            return EventType.KeyPressed;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.KeyPressed);
        }

        private ushort _repeatCount;
    }

    public class KeyReleasedEvent : KeyEvent
    {
        public KeyReleasedEvent(KeyCode keyCode) : base(keyCode)
        {
        }

        public override string ToString()
        {
            return "KeyReleasedEvent: " + _keyCode;
        }

        public static EventType GetStaticType()
        {
            return EventType.KeyReleased;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.KeyReleased);
        }
    }

    public class KeyTypedEvent : KeyEvent
    {
        public KeyTypedEvent(KeyCode keyCode) : base(keyCode)
        {
        }

        public override string ToString()
        {
            return "KeyTypedEvent: " + _keyCode;
        }

        public static EventType GetStaticType()
        {
            return EventType.KeyTyped;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.KeyTyped);
        }
    }
}