using System;
using System.Runtime.InteropServices;
using Engine.EngineCore.Core;
using Engine.EngineCore.Events;
using Engine.EngineCore.Renderer;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = Engine.EngineCore.Core.Window;

namespace Engine.Platform.Windows
{
    public class WindowsWindow : Window
    {
        private static uint _glfwWindowCount = 0;

        static void GLFWErrorCallback(ErrorCode error, string description)
        {
            Console.WriteLine("ERROR: GLFW Error ({0}): {1}", error, description);
        }

        public WindowsWindow(ref WindowProps props)
        {
            Init(ref props);
        }

        ~WindowsWindow()
        {
            Shutdown();
        }

        public override void OnUpdate()
        {
            GLFW.PollEvents();
            _context.SwapBuffers();
        }

        public override uint GetWidth()
        {
            return _data.Width;
        }

        public override uint GetHeight()
        {
            return _data.Height;
        }

        public override void SetEventCallback(Action<Event> callback)
        {
            _data.EventCallback = callback;
        }

        public override void SetVSync(bool enabled)
        {
            if (enabled)
                GLFW.SwapInterval(1);
            else
                GLFW.SwapInterval(0);

            _data.VSync = enabled;
        }

        public override bool IsVSync()
        {
            return _data.VSync;
        }

        public override unsafe void* GetNativeWindow()
        {
            return _window;
        }

        private unsafe void Init(ref WindowProps props)
        {
            _data.Title = props.Title;
            _data.Width = props.Width;
            _data.Height = props.Height;

            if (_glfwWindowCount == 0)
            {
                bool success = GLFW.Init();
                if (!success)
                {
                    throw new ApplicationException("Could not initialize GLFW!");
                }

                GLFW.SetErrorCallback(GLFWErrorCallback);
            }

            {
#if (HZ_DEBUG)
			if (Renderer::GetAPI() == RendererAPI::API::OpenGL)
				glfwWindowHint(GLFW_OPENGL_DEBUG_CONTEXT, GLFW_TRUE);
#endif
                _window = GLFW.CreateWindow((int)props.Width, (int)props.Height, _data.Title, null, null);
                ++_glfwWindowCount;
            }

            _context = GraphicsContext.Create(_window);
            _context.Init();

            var dataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<WindowData>());
            Marshal.StructureToPtr<WindowData>(_data, dataPtr, false);
            GLFW.SetWindowUserPointer(_window, dataPtr.ToPointer());
            SetVSync(true);

            // Set GLFW callbacks
            GLFW.SetWindowSizeCallback(_window, (window, width, height) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));
                data.Width = (uint)width;
                data.Height = (uint)height;

                WindowResizeEvent @event = new((uint)width, (uint)height);
                data.EventCallback(@event);
            });

            GLFW.SetWindowCloseCallback(_window, (window) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));
                WindowCloseEvent @event = new();
                data.EventCallback(@event);
            });

            GLFW.SetKeyCallback(_window, (window, key, scancode, action, mods) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));

                switch (action)
                {
                    case InputAction.Press:
                    {
                        KeyPressedEvent @event = new((KeyCode)key, 0);
                        data.EventCallback(@event);
                        break;
                    }
                    case InputAction.Release:
                    {
                        KeyReleasedEvent @event = new((KeyCode)key);
                        data.EventCallback(@event);
                        break;
                    }
                    case InputAction.Repeat:
                    {
                        KeyPressedEvent @event = new((KeyCode)key, 1);
                        data.EventCallback(@event);
                        break;
                    }
                }
            });

            GLFW.SetCharCallback(_window, (window, keycode) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));

                KeyTypedEvent @event = new((KeyCode)keycode);
                data.EventCallback(@event);
            });

            GLFW.SetMouseButtonCallback(_window, (window, button, action, mods) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));

                switch (action)
                {
                    case InputAction.Press:
                    {
                        MouseButtonPressedEvent @event = new((MouseCode)button);
                        data.EventCallback(@event);
                        break;
                    }
                    case InputAction.Release:
                    {
                        MouseButtonReleasedEvent @event = new((MouseCode)button);
                        data.EventCallback(@event);
                        break;
                    }
                }
            });

            GLFW.SetScrollCallback(_window, (window, xOffset, yOffset) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));

                MouseScrolledEvent @event = new((float)xOffset, (float)yOffset);
                data.EventCallback(@event);
            });

            GLFW.SetCursorPosCallback(_window, (window, xPos, yPos) =>
            {
                WindowData data = Marshal.PtrToStructure<WindowData>((IntPtr)GLFW.GetWindowUserPointer(window));

                MouseMovedEvent @event = new((float)xPos, (float)yPos);
                data.EventCallback(@event);
            });
        }

        private unsafe void Shutdown()
        {
            GLFW.DestroyWindow(_window);
            --_glfwWindowCount;

            if (_glfwWindowCount == 0)
            {
                GLFW.Terminate();
            }
        }

        private unsafe OpenTK.Windowing.GraphicsLibraryFramework.Window* _window;
        private GraphicsContext _context;

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowData
        {
            public string Title;
            public uint Width, Height;
            public bool VSync;

            public Action<Event> EventCallback;
        };

        private WindowData _data;
    }
}