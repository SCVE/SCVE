using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLContext : RenderingContext
    {
        private DebugProc _openGLMessageCallback;
        
        public OpenGLContext(ScveWindow window) : base(window)
        {
            Logger.Warn("Constructing OpenGLContext");
        }

        public override unsafe void Init()
        {
            Logger.Trace($"{nameof(OpenGLContext)}.{nameof(Init)}");
            GL.LoadBindings(new GLFWBindingsContext());
            
            Logger.Warn("OpenGL Info:");
            Logger.Warn("  Vendor: {0}", GL.GetString(StringName.Vendor));
            Logger.Warn("  Renderer: {0}", GL.GetString(StringName.Renderer));
            Logger.Warn("  Version: {0}", GL.GetString(StringName.Version));
            
            _openGLMessageCallback = OnOpenGLDebugMessageCallback;

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback(_openGLMessageCallback, IntPtr.Zero);
            GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DebugSeverityNotification, 0, (int*)IntPtr.Zero, false);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
        }

        private unsafe void OnOpenGLDebugMessageCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
        {
            switch (severity)
            {
                case DebugSeverity.DebugSeverityHigh:
                    Logger.Fatal($"Critical: {new string((char*)message)}");
                    return;
                case DebugSeverity.DebugSeverityMedium:
                    Logger.Error($"Error: {new string((char*)message)}");
                    return;
                case DebugSeverity.DebugSeverityLow:
                    Logger.Warn($"Warn: {new string((char*)message)}");
                    return;
                case DebugSeverity.DebugSeverityNotification:
                case DebugSeverity.DontCare:
                    Logger.Trace($"Trace: {new string((char*)message)}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown severity level!");
            }
        }

        public override unsafe void SwapBuffers()
        {
            Logger.Trace($"{nameof(OpenGLContext)}.{nameof(SwapBuffers)}");
            GLFW.SwapBuffers((Window*)Window.Handle);
        }
    }
}