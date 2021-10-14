using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLRenderer : IRenderer
    {
        private DebugProc _openGLMessageCallback;

        public OpenGLRenderer()
        {
            Logger.Trace("Constructing OpenGLRenderer");
        }
        
        public void OnInit()
        {
            Logger.Trace("OpenTkOpenGLRenderer.OnInit");

            _openGLMessageCallback = OnOpenGLDebugMessageCallback;

            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback(_openGLMessageCallback, IntPtr.Zero);
            GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DebugSeverityNotification, 0, (int[])null, false);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);

            Application.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
        }

        private unsafe void OnOpenGLDebugMessageCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
        {
            var messageString = Marshal.PtrToStringAnsi(message);
            switch (severity)
            {
                case DebugSeverity.DebugSeverityHigh:
                    Logger.Fatal($"Critical: {messageString}");
                    return;
                case DebugSeverity.DebugSeverityMedium:
                    Logger.Error($"Error: {messageString}");
                    return;
                case DebugSeverity.DebugSeverityLow:
                    Logger.Warn($"Warn: {messageString}");
                    return;
                case DebugSeverity.DebugSeverityNotification:
                case DebugSeverity.DontCare:
                    Logger.Trace($"Trace: {messageString}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown severity level!");
            }
        }

        private void InputOnWindowSizeChanged(int width, int height)
        {
            SetViewport(0, 0, width, height);
        }

        public void OnTerminate()
        {
            Logger.Trace("OpenTkOpenGLRenderer.OnTerminate");
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

        public void Render(VertexArray vertexArray, Program shaderProgram)
        {
            Logger.Trace("OpenGLRenderer.Render()");
            shaderProgram.Bind();
            vertexArray.Bind();
            // GL.DrawArrays(PrimitiveType.Triangles, 0, vertexArray.IndexBuffer.Count);
            GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}