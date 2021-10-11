using System;
using System.Collections.Concurrent;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Services;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class GlfwWindowManager : WindowManager
    {
        private ConcurrentDictionary<IntPtr, ScveWindow> _windowsMap = new();

        // NOTE: this wierd callback is required, because GLFW complains about garbage collected delegates if I pass in just a void
        private GLFWCallbacks.WindowCloseCallback _windowCloseRequestedCallback;
        private GLFWCallbacks.ErrorCallback _errorCallback;

        public override unsafe ScveWindow Create(WindowProps props)
        {
            Logger.Warn($"{nameof(GlfwWindowManager)}.{nameof(Create)}");
            var window = GLFW.CreateWindow(props.Width, props.Height, props.Title, null, null);

            GLFW.MakeContextCurrent(window);
            
            GLFW.SetWindowCloseCallback(window, _windowCloseRequestedCallback);

            var windowPtr = (IntPtr)window;
            var glfwWindow = new GlfwWindow(props.Title, props.Width, props.Height, props.IsMain, windowPtr);
            Windows.Add(glfwWindow);

            _windowsMap.TryAdd(windowPtr, glfwWindow);

            if (props.IsMain)
            {
                MainWindow = glfwWindow;
            }

            return glfwWindow;
        }

        private unsafe void OnWindowCloseRequested(Window* window)
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(OnWindowCloseRequested)}");
            var scveWindow = _windowsMap[(IntPtr)window];
            scveWindow.OnClose();
            Close(scveWindow);
        }

        public override unsafe void OnInit()
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(OnInit)}");
            GLFW.Init();
            _windowCloseRequestedCallback = OnWindowCloseRequested;
            _errorCallback = OnGlfwError;
            GLFW.SetErrorCallback(_errorCallback);
            
            Logger.Warn("GLFW Inited");
        }

        private void OnGlfwError(ErrorCode error, string description)
        {
            Logger.Error($"GLFW - {description}");
        }

        public override void PollEvents()
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(PollEvents)}");
            if (!_windowsMap.IsEmpty)
            {
                GLFW.PollEvents();
            }
        }

        public override unsafe bool WindowShouldClose(ScveWindow window)
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(WindowShouldClose)}");
            return GLFW.WindowShouldClose((Window*)window.Handle);
        }

        public override void OnTerminate()
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(OnWindowCloseRequested)}");
            foreach (var windowMap in _windowsMap)
            {
                windowMap.Value.OnClose();
            }

            GLFW.Terminate();
            Console.WriteLine("GLFW Terminated");
        }

        public override unsafe void Close(ScveWindow window)
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(Close)}");

            _windowsMap.TryRemove(window.Handle, out _);
            Windows.Remove(window);

            GLFW.DestroyWindow((Window*)window.Handle);

            if (window.IsMain)
            {
                MainWindow = null;
            }
            
            if (_windowsMap.IsEmpty)
            {
                Application.Instance.RequestTerminate();
            }
        }

        public override unsafe void SwapBuffers(ScveWindow window)
        {
            GLFW.SwapBuffers((Window*)window.Handle);
        }
    }
}