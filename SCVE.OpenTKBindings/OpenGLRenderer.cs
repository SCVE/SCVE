using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Services;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLRenderer : IRenderer
    {
        private DebugProc OpenGLMessageCallback;

        public void OnInit()
        {
            Logger.Warn("OpenTkOpenGLRenderer.OnInit");
            GL.DebugMessageCallback(OpenGLMessageCallback, IntPtr.Zero);
        }

        public void OnTerminate()
        {
            Logger.Warn("OpenTkOpenGLRenderer.OnTerminate");
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void SetClearColor(Color color)
        {
            GL.ClearColor(color.R, color.G, color.B, color.A);
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }
    }
}