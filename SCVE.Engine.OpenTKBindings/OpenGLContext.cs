using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.OpenTKBindings
{
    public class OpenGLContext : Context
    {
        public OpenGLContext(ScveWindow window) : base(window)
        {
            Logger.Construct(nameof(OpenGLContext));
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