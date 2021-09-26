using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ErrorCode = OpenTK.Windowing.GraphicsLibraryFramework.ErrorCode;
using PrimitiveType = OpenTK.Graphics.OpenGL.Compatibility.PrimitiveType;

namespace OpenTKTests
{
    public static class Program
    {
        private static unsafe void KeyCallback(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
        {
            if (key == Keys.Escape && action == InputAction.Press)
            {
                GLFW.SetWindowShouldClose(window, true);
            }
        }

        private static unsafe void Main(string[] args)
        {
            
            
            
            GLFW.SetErrorCallback(ErrorCallback);

            if (!GLFW.Init())
            {
                GLFW.Terminate();
            }

            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 2);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 0);

            Window* window = GLFW.CreateWindow(640, 480, "Hello world", null, null);

            if (window is null)
            {
                GLFW.Terminate();
            }

            GLFW.SetKeyCallback(window, KeyCallback);
            GLFW.SetWindowSizeCallback(window, WindowSizeCallback);
            GLFW.SetWindowCloseCallback(window, CloseCallback);

            GLFW.MakeContextCurrent(window);

            GLFW.SwapInterval(1);

            GLLoader.LoadBindings(new GLFWBindingsContext());

            while (!GLFW.WindowShouldClose(window))
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                OpenTK.Graphics.OpenGL.Compatibility.GL.Begin(PrimitiveType.Triangles);

                OpenTK.Graphics.OpenGL.Compatibility.GL.Vertex2f(-0.5f, -0.5f);
                OpenTK.Graphics.OpenGL.Compatibility.GL.Vertex2f(0f, 0.5f);
                OpenTK.Graphics.OpenGL.Compatibility.GL.Vertex2f(0.5f, -0.5f);

                OpenTK.Graphics.OpenGL.Compatibility.GL.End();

                GLFW.SwapBuffers(window);
                GLFW.PollEvents();
            }

            GLFW.DestroyWindow(window);

            GLFW.Terminate();
        }

        private static unsafe void CloseCallback(Window* window)
        {
            GLFW.SetWindowShouldClose(window, true);
        }

        private static unsafe void WindowSizeCallback(Window* window, int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        private static unsafe void ErrorCallback(ErrorCode error, string description)
        {
            Console.WriteLine($"{error} - {description}");
        }
    }
}