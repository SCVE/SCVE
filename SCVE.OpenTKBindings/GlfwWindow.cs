using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class GlfwWindow : ScveWindow
    {
        private static int _glfwWindowCount = 0;

        private unsafe Window* _window;

        // NOTE: this weird callback is required, because GLFW complains about garbage collected delegates if I pass in just a void
        private GLFWCallbacks.WindowCloseCallback _windowCloseCallback;
        private GLFWCallbacks.ErrorCallback _errorCallback;
        private GLFWCallbacks.WindowSizeCallback _sizeCallback;
        private GLFWCallbacks.KeyCallback _keyCallback;
        private GLFWCallbacks.CharCallback _charCallback;
        private GLFWCallbacks.MouseButtonCallback _mouseButtonCallback;
        private GLFWCallbacks.ScrollCallback _scrollCallback;
        private GLFWCallbacks.CursorPosCallback _cursorPosCallback;
        private GLFWCallbacks.CursorEnterCallback _cursorEnterCallback;
        private GLFWCallbacks.WindowIconifyCallback _windowIconifyCallback;
        private GLFWCallbacks.WindowMaximizeCallback _windowMaximizeCallback;

        public unsafe GlfwWindow(WindowProps props) : base(props)
        {
            Logger.Warn("Constructing GlfwWindow");

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
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnWindowMaximized)}(maximized: {maximized})");
        }

        private unsafe void OnWindowMinimized(Window* window, bool iconified)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnWindowMinimized)}(iconified: {iconified})");
        }

        public override unsafe void Shutdown()
        {
            GLFW.DestroyWindow(_window);
            _glfwWindowCount--;
            if (_glfwWindowCount == 0)
            {
                GLFW.Terminate();
            }
        }

        public override void OnUpdate()
        {
            GLFW.PollEvents();
            Context.SwapBuffers();
        }

        public override unsafe void SetTitle(string title)
        {
            base.SetTitle(title);
            GLFW.SetWindowTitle(_window, title);
        }

        private unsafe void OnCursorEnter(Window* window, bool entered)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnCursorEnter)}(entered: {entered})");
        }

        private unsafe void OnCursorMoved(Window* window, double x, double y)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnCursorMoved)}(x: {x}, y: {y})");
        }

        private unsafe void OnScroll(Window* window, double offsetx, double offsety)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnScroll)}(offsetx: {offsetx}, offsety: {offsety})");
        }

        private unsafe void OnMouseButtonClick(Window* window, MouseButton button, InputAction action, KeyModifiers mods)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnMouseButtonClick)}(button: {button}, action: {action}, mods: {mods})");
        }

        private unsafe void OnKeyTyped(Window* window, uint codepoint)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnKeyTyped)}(codepoint: {codepoint})");
        }

        private unsafe void OnKeyPressed(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnKeyPressed)}(scancode: {scancode}, action: {action}, key: {key}, mods: {mods})");
        }

        private unsafe void OnWindowClose(Window* window)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnWindowClose)}()");
            Application.Instance.RequestTerminate();
        }

        private unsafe void OnWindowSizeChanged(Window* window, int width, int height)
        {
            Logger.Warn($"{nameof(GlfwWindow)}.{nameof(OnWindowSizeChanged)}(width: {width}, height: {height})");
        }

        public override void SetVSync(bool vSync)
        {
            GLFW.SwapInterval(vSync ? 1 : 0);
        }

        private static void OnGlfwError(ErrorCode error, string description)
        {
            Logger.Error($"GLFW - {description}");
        }
    }
}