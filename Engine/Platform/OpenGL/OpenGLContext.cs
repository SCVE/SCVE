using System;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Engine.Platform.OpenGL
{
    public class OpenGLContext : GraphicsContext
    {
        public unsafe OpenGLContext(Window* windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public override unsafe void Init()
        {
            GLFW.MakeContextCurrent(_windowHandle);
            GLLoader.LoadBindings(new GLFWBindingsContext());
            
            Console.WriteLine("OpenGL Info");
            Console.WriteLine($"  Version: {GL.GetString(StringName.Version)}");
            Console.WriteLine($"  Renderer: {GL.GetString(StringName.Renderer)}");
            Console.WriteLine($"  Vendor: {GL.GetString(StringName.Vendor)}");
            Console.WriteLine($"  ShadingLanguageVersion: {GL.GetString(StringName.ShadingLanguageVersion)}");
            
            GLFW.GetVersion(out var major, out var minor, out var revision);

            if (major <= 4 && (major != 4 || minor < 5))
            {
                throw new ApplicationException("Hazel requires at least OpenGL version 4.5!");
            }
        }

        public override unsafe void SwapBuffers()
        {
            GLFW.SwapBuffers(_windowHandle);
        }
        
        private unsafe Window* _windowHandle;
    }
}