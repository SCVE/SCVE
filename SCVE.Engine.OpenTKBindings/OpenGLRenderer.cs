using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Primitives;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.OpenTKBindings
{
    public class OpenGLRenderer : IRenderer
    {
        private DebugProc _openGLMessageCallback;

        public OpenGLRenderer()
        {
            Logger.Construct(nameof(OpenGLRenderer));
        }

        public void Init()
        {
            Logger.Warn("OpenGLRenderer.OnInit");

            _openGLMessageCallback = OnOpenGLDebugMessageCallback;

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback(_openGLMessageCallback, IntPtr.Zero);
            GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DebugSeverityNotification, 0, (int[])null, false);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // NOTE: https://habr.com/ru/post/342610/
            // GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Multisample);

            ScveEngine.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
        }

        private void OnOpenGLDebugMessageCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
        {
            var messageString = Marshal.PtrToStringAnsi(message);
            switch (severity)
            {
                case DebugSeverity.DebugSeverityHigh:
                    Logger.Fatal($"OpenGL - {messageString}");
                    return;
                case DebugSeverity.DebugSeverityMedium:
                    Logger.Error($"OpenGL - {messageString}");
                    return;
                case DebugSeverity.DebugSeverityLow:
                    Logger.Warn($"OpenGL - {messageString}");
                    return;
                case DebugSeverity.DebugSeverityNotification:
                case DebugSeverity.DontCare:
                    Logger.Trace($"OpenGL - {messageString}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown severity level!");
            }
        }

        private void InputOnWindowSizeChanged(int width, int height)
        {
            SetFromWindow(ScveEngine.Instance.MainWindow);
        }

        public void SetFromWindow(ScveWindow window)
        {
            SetViewport(0, 0, window.Width, window.Height);
        }

        public void OnTerminate()
        {
            Logger.Trace("OpenTkOpenGLRenderer.OnTerminate");
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public void RenderSolid(VertexArray vertexArray, ShaderProgram shaderProgram)
        {
            Logger.Trace("OpenGLRenderer.RenderSolid()");
            vertexArray.Bind();
            shaderProgram.Bind();
            // GL.DrawArrays(PrimitiveType.Triangles, 0, vertexArray.IndexBuffer.Count);
            GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void RenderWireframe(VertexArray vertexArray, ShaderProgram shaderProgram)
        {
            Logger.Trace("OpenGLRenderer.RenderWireframe()");
            vertexArray.Bind();
            shaderProgram.Bind();

            // GL.DrawArrays(PrimitiveType.Triangles, 0, vertexArray.IndexBuffer.Count);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
    }
}