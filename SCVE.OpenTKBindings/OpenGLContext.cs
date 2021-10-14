using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLContext : Context
    {
        private DebugProc _openGLMessageCallback;

        public OpenGLContext(ScveWindow window) : base(window)
        {
            Logger.Trace("Constructing OpenGLContext");
        }

        public override void Init()
        {
            Logger.Trace("OpenGLContext.Init()");
            GL.LoadBindings(new GLFWBindingsContext());

            Logger.Warn("OpenGL Info:");
            Logger.Warn("  Vendor: {0}", GL.GetString(StringName.Vendor));
            Logger.Warn("  Renderer: {0}", GL.GetString(StringName.Renderer));
            Logger.Warn("  Version: {0}", GL.GetString(StringName.Version));
        }

        public override unsafe void SwapBuffers()
        {
            Logger.Trace($"OpenGLContext.SwapBuffers()");
            GLFW.SwapBuffers((Window*)Window.Handle);
        }
    }
}