using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SCVE.Core.Entities;
using SCVE.Core.Services;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLRenderer : IRenderer
    {
        public void OnDeferInit()
        {
            Logger.Warn("OpenTkOpenGLRenderer.Init");
            GL.LoadBindings(new GLFWBindingsContext());
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
    }
}