namespace Engine.EngineCore.Events
{
    public class WindowResizeEvent : Event
    {
        public WindowResizeEvent(uint width, uint height)
        {
            _width = width;
            _height = height;
        }

        public uint GetWidth()
        {
            return _width;
        }

        public uint GetHeight()
        {
            return _height;
        }

        public override string ToString()
        {
            return "WindowResizeEvent: " + _width + ", " + _height;
        }

        public static EventType GetStaticType()
        {
            return EventType.WindowResize;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.WindowResize);
        }

        public override int GetCategoryFlags()
        {
            return (int)EventCategory.EventCategoryApplication;
        }

        private uint _width;
        private uint _height;
    }

    public class WindowCloseEvent : Event
    {
        public static EventType GetStaticType()
        {
            return EventType.WindowClose;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.WindowClose);
        }

        public override int GetCategoryFlags()
        {
            return (int)EventCategory.EventCategoryApplication;
        }
    }

    public class AppTickEvent : Event
    {
        public static EventType GetStaticType()
        {
            return EventType.AppTick;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.AppTick);
        }

        public override int GetCategoryFlags()
        {
            return (int)EventCategory.EventCategoryApplication;
        }
    }

    public class AppUpdateEvent : Event
    {
        public static EventType GetStaticType()
        {
            return EventType.AppUpdate;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.AppUpdate);
        }

        public override int GetCategoryFlags()
        {
            return (int)EventCategory.EventCategoryApplication;
        }
    }

    public class AppRenderEvent : Event
    {
        public static EventType GetStaticType()
        {
            return EventType.AppRender;
        }

        public override EventType GetEventType()
        {
            return GetStaticType();
        }

        public override string GetName()
        {
            return nameof(EventType.AppRender);
        }

        public override int GetCategoryFlags()
        {
            return (int)EventCategory.EventCategoryApplication;
        }
    }
}