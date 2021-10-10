using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.Entities;
using SCVE.Core.Services;

namespace SCVE.OpenTK.Glfw
{
    public class GlfwWindowManager : WindowManager
    {
        private Dictionary<IntPtr, ScveWindow> _windowsMap = new();

        public override unsafe ScveWindow Create(WindowProps props)
        {
            var window = GLFW.CreateWindow(props.Width, props.Height, props.Title, null, null);

            GLFW.SetWindowCloseCallback(window, OnWindowClose);

            var windowPtr = (IntPtr)window;
            var glfwWindow = new GlfwWindow(props.Title, props.Width, props.Height, windowPtr);
            Windows.Add(glfwWindow);

            _windowsMap.Add(windowPtr, glfwWindow);

            return glfwWindow;
        }

        private unsafe void OnWindowClose(Window* window)
        {
            _windowsMap[(IntPtr)window].Close();
        }

        public override void Init()
        {
            GLFW.Init();
            Console.WriteLine("GLFW Inited");
        }

        public override void PollEvents()
        {
            if (_windowsMap.Any())
                GLFW.PollEvents();
        }

        public override unsafe bool WindowShouldClose(ScveWindow window)
        {
            return GLFW.WindowShouldClose((Window*)window.Handle);
        }

        public override unsafe void Close(ScveWindow window)
        {
            _windowsMap.Remove(window.Handle);
            Windows.Remove(window);
            GLFW.DestroyWindow((Window*)window.Handle);
        }

        public override void Terminate()
        {
            GLFW.Terminate();
            Console.WriteLine("GLFW Terminated");
        }
    }
}