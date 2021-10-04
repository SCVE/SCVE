using System;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Engine.Platform
{
    public class OpenGLRendererAPI : RendererAPI
    {
        public override void Init()
        {
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
            GL.DebugMessageCallback(OpenGLMessageCallback, IntPtr.Zero);
            GL.DebugMessageControl(DebugSource.DontCare, DebugType.DontCare, DebugSeverity.DebugSeverityNotification, 0, 0, false);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
        }

        private void OpenGLMessageCallback(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length, IntPtr message, IntPtr userparam)
        {
            switch (severity)
            {
                case DebugSeverity.DebugSeverityHigh:
                    Console.WriteLine($"Critical: {message}");
                    return;
                case DebugSeverity.DebugSeverityMedium:
                    Console.WriteLine($"Error: {message}");
                    return;
                case DebugSeverity.DebugSeverityLow:
                    Console.WriteLine($"Warn: {message}");
                    return;
                case DebugSeverity.DebugSeverityNotification:
                    Console.WriteLine($"Trace: {message}");
                    return;
                default:
                    throw new ArgumentOutOfRangeException("Unknown severity level!");
            }
        }

        public override void SetViewport(uint x, uint y, uint width, uint height)
        {
            GL.Viewport((int)x, (int)y, (int)width, (int)height);
        }

        public override void SetClearColor(Vector4 color)
        {
            GL.ClearColor(color.X, color.Y, color.Z, color.W);
        }

        public override void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public override void DrawIndexed(VertexArray vertexArray, uint indexCount)
        {
            uint count = indexCount != 0 ? indexCount : vertexArray.GetIndexBuffer().GetCount();
            GL.DrawElements(PrimitiveType.Triangles, (int)count, DrawElementsType.UnsignedInt, 0);
            GL.BindTexture(TextureTarget.Texture2d, TextureHandle.Zero);
        }
    }
}