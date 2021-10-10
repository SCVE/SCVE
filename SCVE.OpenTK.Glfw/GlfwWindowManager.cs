using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Services;
using SCVE.Core.Utilities;

namespace SCVE.OpenTK.Glfw
{
    public class GlfwWindowManager : WindowManager
    {
        private ConcurrentDictionary<IntPtr, ScveWindow> _windowsMap = new();

        public override unsafe ScveWindow Create(WindowProps props)
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(Create)}");
            var window = GLFW.CreateWindow(props.Width, props.Height, props.Title, null, null);
            GLFW.SetWindowCloseCallback(window, OnWindowCloseRequested);

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
            GLFW.SetWindowShouldClose(window, false);
            var scveWindow = _windowsMap[(IntPtr)window];
            scveWindow.OnClose();
            Close(scveWindow);
        }

        public override void OnInit()
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(OnInit)}");
            GLFW.Init();
            Console.WriteLine("GLFW Inited");
        }

        public override void PollEvents()
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(PollEvents)}");
            if (_windowsMap.Any())
                GLFW.PollEvents();
        }

        public override unsafe bool WindowShouldClose(ScveWindow window)
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(WindowShouldClose)}");
            return GLFW.WindowShouldClose((Window*)window.Handle);
        }

        public override unsafe void Close(ScveWindow window)
        {
            Logger.Trace($"{nameof(GlfwWindowManager)}.{nameof(Close)}");
            _windowsMap.TryRemove(window.Handle, out _);
            Windows.Remove(window);
            
            GLFW.DestroyWindow((Window*)window.Handle);

            if (_windowsMap.Count == 0)
            {
                Application.Instance.RequestTerminate();
            }
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
    }
}