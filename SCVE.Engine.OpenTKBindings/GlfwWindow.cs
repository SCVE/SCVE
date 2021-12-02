using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Utilities;
using static OpenTK.Windowing.GraphicsLibraryFramework.GLFWCallbacks;

namespace SCVE.Engine.OpenTKBindings
{
    public class GlfwWindow : ScveWindow
    {
        private static int _glfwWindowCount = 0;

        private unsafe Window* _window;

        // NOTE: this weird callback is required, because GLFW complains about garbage collected delegates if I pass in just a void
        private WindowCloseCallback _windowCloseCallback;
        private ErrorCallback _errorCallback;
        private WindowSizeCallback _sizeCallback;
        private KeyCallback _keyCallback;
        private CharCallback _charCallback;
        private MouseButtonCallback _mouseButtonCallback;
        private ScrollCallback _scrollCallback;
        private CursorPosCallback _cursorPosCallback;
        private CursorEnterCallback _cursorEnterCallback;
        private WindowIconifyCallback _windowIconifyCallback;
        private WindowMaximizeCallback _windowMaximizeCallback;

        public unsafe GlfwWindow(WindowProps props) : base(props)
        {
            Logger.Construct(nameof(GlfwWindow));

            if (_glfwWindowCount == 0)
            {
                GLFW.Init();
                _errorCallback = OnGlfwError;
                _sizeCallback = OnWindowSizeChanged;
                _windowCloseCallback = OnWindowClose;
                _keyCallback = OnKeyPressed;
                _charCallback = OnKeyTyped;
                _mouseButtonCallback = OnMouseButtonClick;
                _scrollCallback = OnScroll;
                _cursorPosCallback = OnCursorMoved;
                _cursorEnterCallback = OnCursorEnter;
                _windowIconifyCallback = OnWindowMinimized;
                _windowMaximizeCallback = OnWindowMaximized;
                GLFW.SetErrorCallback(_errorCallback);
            }

            _window = GLFW.CreateWindow(props.Width, props.Height, props.Title, null, null);
            
            GLFW.WindowHint(WindowHintInt.Samples, 4);
            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
            
            Handle = new IntPtr(_window);
            GLFW.MakeContextCurrent(_window);
            _glfwWindowCount++;

            Context = new OpenGLContext(this);
            Context.Init();

            SetVSync(true);

            GLFW.SetWindowSizeCallback(_window, _sizeCallback);
            GLFW.SetWindowCloseCallback(_window, _windowCloseCallback);
            GLFW.SetKeyCallback(_window, _keyCallback);
            GLFW.SetCharCallback(_window, _charCallback);
            GLFW.SetMouseButtonCallback(_window, _mouseButtonCallback);
            GLFW.SetScrollCallback(_window, _scrollCallback);
            GLFW.SetCursorPosCallback(_window, _cursorPosCallback);
            GLFW.SetCursorEnterCallback(_window, _cursorEnterCallback);
            GLFW.SetWindowIconifyCallback(_window, _windowIconifyCallback);
            GLFW.SetWindowMaximizeCallback(_window, _windowMaximizeCallback);
        }

        private unsafe void OnWindowMaximized(Window* window, bool maximized)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnWindowMaximized)}(maximized: {maximized})");
            if (maximized)
            {
                ScveEngine.Instance.Input.RegisterWindowMaximized();
            }
            else
            {
                ScveEngine.Instance.Input.RegisterWindowWindowed();
            }
        }

        private unsafe void OnWindowMinimized(Window* window, bool iconified)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnWindowMinimized)}(iconified: {iconified})");
            if (iconified)
            {
                ScveEngine.Instance.Input.RegisterWindowMinimized();
            }
            else
            {
                ScveEngine.Instance.Input.RegisterWindowWindowed();
            }
        }

        public override unsafe void Shutdown()
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(Shutdown)}()");
            GLFW.DestroyWindow(_window);
            _glfwWindowCount--;
            if (_glfwWindowCount == 0)
            {
                GLFW.Terminate();
            }
        }

        public override void OnUpdate()
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnUpdate)}()");
            GLFW.PollEvents();
            Context.SwapBuffers();
        }

        public override unsafe void SetTitle(string title)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(SetTitle)}()");
            base.SetTitle(title);
            GLFW.SetWindowTitle(_window, title);
        }

        private unsafe void OnCursorEnter(Window* window, bool entered)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnCursorEnter)}(entered: {entered})");

            if (entered)
            {
                ScveEngine.Instance.Input.RegisterCursorEnter();
            }
            else
            {
                ScveEngine.Instance.Input.RegisterCursorLeave();
            }
        }

        private unsafe void OnCursorMoved(Window* window, double x, double y)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnCursorMoved)}(x: {x}, y: {y})");

            ScveEngine.Instance.Input.RegisterCursorMoved((float)x, (float)y);
        }

        private unsafe void OnScroll(Window* window, double offsetx, double offsety)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnScroll)}(offsetx: {offsetx}, offsety: {offsety})");

            ScveEngine.Instance.Input.RegisterScroll((float)offsetx, (float)offsety);
        }

        private unsafe void OnMouseButtonClick(Window* window, MouseButton button, InputAction action, KeyModifiers mods)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnMouseButtonClick)}(button: {button}, action: {action}, mods: {mods})");

            switch (action)
            {
                case InputAction.Press:
                    ScveEngine.Instance.Input.RegisterMouseButtonDown(button.ToScveCode());
                    break;
                case InputAction.Release:
                    ScveEngine.Instance.Input.RegisterMouseButtonUp(button.ToScveCode());
                    break;
                case InputAction.Repeat:
                    // This is never called
                    // TODO: figure out what to do with this thing
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        private unsafe void OnKeyTyped(Window* window, uint codepoint)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnKeyTyped)}(codepoint: {codepoint})");
            
            // TODO: figure out what to do with this thing
        }

        private unsafe void OnKeyPressed(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnKeyPressed)}(scancode: {scancode}, action: {action}, key: {key}, mods: {mods})");

            switch (action)
            {
                case InputAction.Press:
                    ScveEngine.Instance.Input.RegisterKeyDown(key.ToScveCode());
                    break;
                case InputAction.Release:
                    ScveEngine.Instance.Input.RegisterKeyUp(key.ToScveCode());
                    break;
                case InputAction.Repeat:
                    ScveEngine.Instance.Input.RegisterKeyRepeat(key.ToScveCode());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        private unsafe void OnWindowClose(Window* window)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnWindowClose)}()");
            ScveEngine.Instance.RequestTerminate();
        }

        private unsafe void OnWindowSizeChanged(Window* window, int width, int height)
        {
            Logger.Trace($"{nameof(GlfwWindow)}.{nameof(OnWindowSizeChanged)}(width: {width}, height: {height})");

            Width = width;
            Height = height;
            
            ScveEngine.Instance.Input.RegisterWindowSizeChanged(width, height);
        }

        public override void SetVSync(bool vSync)
        {
            GLFW.SwapInterval(vSync ? 1 : 0);
        }

        private static void OnGlfwError(ErrorCode error, string description)
        {
            Logger.Error($"GLFW - {description}");
            ScveEngine.Instance.RequestTerminate();
        }
    }
}