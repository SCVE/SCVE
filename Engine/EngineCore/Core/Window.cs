using System;
using Engine.EngineCore.Events;

namespace Engine.EngineCore.Core
{
    public struct WindowProps
    {
        public string Title;
        public uint Width;
        public uint Height;

        public WindowProps(string title = "Hazel Engine", uint width = 1600, uint height = 900)
        {
            Title = title;
            Width = width;
            Height = height;
        }
    };

    public abstract class Window
    {
        public abstract void OnUpdate();

        public abstract uint GetWidth();
        public abstract uint GetHeight();

        // Window attributes
        public abstract  void SetEventCallback(Action<Event> callback);
        public abstract void SetVSync(bool enabled);
        public abstract bool IsVSync();

        public abstract unsafe void* GetNativeWindow();

        public static Window Create(WindowProps props = new())
        {
        }
    }
}