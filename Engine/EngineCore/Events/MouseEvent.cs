using Engine.EngineCore.Core;

namespace Engine.EngineCore.Events
{
    public class MouseMovedEvent : Event
    {
        public MouseMovedEvent(float mouseX, float mouseY)
        {
            _mouseX = mouseX;
            _mouseY = mouseY;
        }

        public float GetX()
        {
            return _mouseX;
        }

        public float GetY()
        {
            return _mouseY;
        }

        public override string ToString()
        {
            return "MouseMovedEvent: " + _mouseX + ", " + _mouseY;
        }

        public static EventType GetStaticType()
        {
            return EventType.MouseMoved;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.MouseMoved);
        }

        public override int GetCategoryFlags()
        {
            return (int)(EventCategory.EventCategoryMouse | EventCategory.EventCategoryInput);
        }

        private float _mouseX;
        private float _mouseY;
    }

    public class MouseScrolledEvent : Event
    {
        public MouseScrolledEvent(float xOffset, float offsetY)
        {
            _xOffset = xOffset;
            _offsetY = offsetY;
        }

        public float GetXOffset()
        {
            return _xOffset;
        }

        public float GetYOffset()
        {
            return _offsetY;
        }

        public override string ToString()
        {
            return "MouseScrolledEvent: " + _xOffset + ", " + _offsetY;
        }

        public static EventType GetStaticType()
        {
            return EventType.MouseScrolled;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.MouseScrolled);
        }

        public override int GetCategoryFlags()
        {
            return (int)(EventCategory.EventCategoryMouse | EventCategory.EventCategoryInput);
        }

        private float _xOffset;
        private float _offsetY;
    }

    public class MouseButtonEvent : Event
    {
        public MouseCode GetMouseButton()
        {
            return _button;
        }

        public override int GetCategoryFlags()
        {
            return (int)(EventCategory.EventCategoryMouse | EventCategory.EventCategoryInput | EventCategory.EventCategoryMouseButton);
        }

        protected MouseButtonEvent(MouseCode button)
        {
            _button = button;
        }

        protected MouseCode _button;
    }

    public class MouseButtonPressedEvent : MouseButtonEvent
    {
        public MouseButtonPressedEvent(MouseCode button)
            : base(button)
        {
        }

        public override string ToString()
        {
            return "MouseButtonPressedEvent: " + _button;
        }

        public static EventType GetStaticType()
        {
            return EventType.MouseButtonPressed;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.MouseButtonPressed);
        }
    }

    public class MouseButtonReleasedEvent : MouseButtonEvent
    {
        public MouseButtonReleasedEvent(MouseCode button)
            : base(button)
        {
        }

        public override string ToString()
        {
            return "MouseButtonReleasedEvent: " + _button;
        }

        public static EventType GetStaticType()
        {
            return EventType.MouseButtonReleased;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.MouseButtonReleased);
        }
    }
}