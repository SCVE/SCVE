using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Loading;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLRenderer : IRenderer
    {
        // This is a cached TextureCoordinates, so we don't allocate them every draw
        // We have 4 texture coordinate for each character (quad) of 2 floats each
        private static readonly float[] TextureCoordinates = new float[4 * 2];

        private DebugProc _openGLMessageCallback;

        private VertexArray _positiveUnitVertexArray;
        private VertexArray _lineVertexArray;

        private ShaderProgram _flatColorShaderProgram;
        private ShaderProgram _lineShaderProgram;
        private ShaderProgram _textShaderProgram;

        private ScveMatrix4X4 _scaleMatrix = ScveMatrix4X4.Identity;
        private ScveMatrix4X4 _translationMatrix = ScveMatrix4X4.Identity;

        public ScveMatrix4X4 ModelMatrix { get; private set; } = ScveMatrix4X4.Identity;
        public ScveMatrix4X4 ViewMatrix { get; private set; } = ScveMatrix4X4.Identity;
        public ScveMatrix4X4 ProjectionMatrix { get; private set; } = ScveMatrix4X4.Identity;
        public ScveMatrix4X4 ViewProjectionMatrix { get; private set; } = ScveMatrix4X4.Identity;

        public OpenGLRenderer()
        {
            Logger.Construct(nameof(OpenGLRenderer));
        }

        public void OnInit()
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

            Application.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;

            _positiveUnitVertexArray = CreatePositiveUnitVertexArray();
            _lineVertexArray = CreateLineVertexArray();

            _flatColorShaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("FlatColor");
            _lineShaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("Line");
            _textShaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("Text2D");
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
            SetFromWindow(Application.Instance.MainWindow);
        }

        public void SetFromWindow(ScveWindow window)
        {
            SetViewport(0, 0, window.Width, window.Height);

            ProjectionMatrix
                .MakeIdentity()
                .MakeOrthographicOffCenter(
                    left: 0,
                    right: window.Width,
                    bottom: window.Height,
                    top: 0,
                    depthNear: -1,
                    depthFar: 1
                );

            ViewProjectionMatrix.MakeIdentity().Multiply(ProjectionMatrix).Multiply(ViewMatrix);
        }

        public void OnTerminate()
        {
            Logger.Trace("OpenTkOpenGLRenderer.OnTerminate");
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void SetClearColor(ColorRgba colorRgba)
        {
            GL.ClearColor(colorRgba.R, colorRgba.G, colorRgba.B, colorRgba.A);
        }

        public void SetViewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public ScveMatrix4X4 GetViewMatrix()
        {
            return ViewMatrix;
        }

        public ScveMatrix4X4 GetProjectionMatrix()
        {
            return ProjectionMatrix;
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

        public void RenderColorRect(float x, float y, float width, float height, ColorRgba colorRgba)
        {
            _flatColorShaderProgram.SetVector4("u_Color", colorRgba.R, colorRgba.G, colorRgba.B, colorRgba.A);

            ModelMatrix.MakeIdentity().MakeScale(width, height).MakeTranslation(x, y);

            _flatColorShaderProgram.SetMatrix4("u_Model",
                ModelMatrix
            );
            _flatColorShaderProgram.SetMatrix4("u_View",
                ViewMatrix
            );
            _flatColorShaderProgram.SetMatrix4("u_Proj",
                ProjectionMatrix
            );

            RenderSolid(_positiveUnitVertexArray, _flatColorShaderProgram);
        }

        public void RenderLine(float x1, float y1, float x2, float y2, ColorRgba colorRgba, float width = 1)
        {
            Logger.Trace("OpenGLRenderer.RenderLine()");

            float[] vertices =
            {
                x1, y1, 0f,
                x2, y2, 0f,
            };

            _lineVertexArray.VertexBuffers[0].Upload(vertices, BufferUsage.Stream);

            _lineShaderProgram.SetVector4("u_Color", colorRgba.R, colorRgba.G, colorRgba.B, colorRgba.A);

            _lineShaderProgram.SetMatrix4("u_Proj",
                ProjectionMatrix
            );
            _lineVertexArray.Bind();

            GL.LineWidth(width);

            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        }
        
        public void RenderText(ScveFont font, string text, float fontSize, float x, float y)
        {
            float destLineHeight = Maths.FontSizeToLineHeight(fontSize);
            float lineHeightRel = destLineHeight / font.LineHeight;
            float xStart = x;

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    y += destLineHeight;
                    x = xStart;
                    continue;
                }

                if (text[i] == 't')
                {
                    int a = 5;
                }

                FontAtlasChunk chunk;
                if (font.Atlas.Chunks.ContainsKey(text[i]))
                {
                    chunk = font.Atlas.Chunks[(int)text[i]];
                }
                else
                {
                    chunk = font.Atlas.Chunks[(int)'?'];
                }

                // NOTE: we invert the Y axis, because Textures have (0,0) at top left, but OpenGL have (0,0) at bottom left
                float textureLeft = (float)chunk.TextureCoordX / font.Texture.Width;
                float textureTop = 1 - (float)chunk.TextureCoordY / font.Texture.Height;
                float textureRight = (float)(chunk.TextureCoordX + font.Atlas.ChunkSize) / font.Texture.Width;
                float textureBottom = 1 - (float)(chunk.TextureCoordY + font.Atlas.ChunkSize) / font.Texture.Height;


                // NOTE: When we overlap triangles with same Z and have a Z-Depth Test, OpenGL have a collision, and stops rendering them correctly
                // So I simply add a small Z offset to each character quad (some kind of stack), so OpenGL can sort the quads and render them correctly
                // Visible like this [[[[  ] instead of  [[[]]]
                // Take this into account when enabling DEPTH_TEST
                float zOffset = 0f; //i * 0.005f;

                // top left
                TextureCoordinates[0] = textureLeft;
                TextureCoordinates[1] = textureTop;

                // top right
                TextureCoordinates[2] = textureRight;
                TextureCoordinates[3] = textureTop;

                // Bottom right
                TextureCoordinates[4] = textureRight;
                TextureCoordinates[5] = textureBottom;

                // Bottom left
                TextureCoordinates[6] = textureLeft;
                TextureCoordinates[7] = textureBottom;

                _positiveUnitVertexArray.VertexBuffers[1].Upload(TextureCoordinates, BufferUsage.Dynamic);

                _scaleMatrix.MakeIdentity().MakeScale(destLineHeight, destLineHeight);
                _translationMatrix.MakeIdentity().MakeTranslation(
                    x + chunk.BearingX * lineHeightRel,
                    // Here we offset from top of line to bottom, then from bottom we go up for CellDescent (baseline offset) + BearingY (size of a char)
                    y + destLineHeight - font.Atlas.CellDescent * lineHeightRel - chunk.BearingY * lineHeightRel
                );

                ModelMatrix.MakeIdentity().Multiply(_scaleMatrix).Multiply(_translationMatrix);
                _textShaderProgram.SetMatrix4("u_Model",
                    ModelMatrix
                );
                _textShaderProgram.SetMatrix4("u_Proj",
                    ProjectionMatrix
                );
                font.Texture.Bind(0);
                _textShaderProgram.Bind();

                RenderSolid(_positiveUnitVertexArray, _textShaderProgram);

                x += chunk.Advance * lineHeightRel;
            }
        }

        // TODO: Extract clip component
        public void RenderText(ScveFont font, string text, float fontSize, float x, float y, float clipWidth, float clipHeight)
        {
            GL.Scissor((int)x, (int)(Application.Instance.MainWindow.Height - (int)clipHeight - y), (int)clipWidth, (int)clipHeight);
            RenderText(font, text, fontSize, x, y);
            GL.Scissor(0, 0, Application.Instance.MainWindow.Width, Application.Instance.MainWindow.Height);
        }

        private static VertexArray CreatePositiveUnitVertexArray()
        {
            VertexArray vertexArray = new OpenGLVertexArray();
            var rectGeometry = GeometryGenerator.GeneratePositiveUnitSquare();

            var verticesVertexBuffer = new OpenGLVertexBuffer(rectGeometry.Vertices, BufferUsage.Static)
            {
                Layout = new VertexBufferLayout(new()
                {
                    new(VertexBufferElementType.Float3, "a_Position")
                })
            };

            vertexArray.AddVertexBuffer(verticesVertexBuffer);

            var textureCoordinateVertexBuffer = new OpenGLVertexBuffer(rectGeometry.Vertices, BufferUsage.Dynamic)
            {
                Layout = new VertexBufferLayout(new()
                {
                    new(VertexBufferElementType.Float2, "a_TextureCoordinate")
                })
            };

            vertexArray.AddVertexBuffer(textureCoordinateVertexBuffer);

            var indexBuffer = new OpenGLIndexBuffer(rectGeometry.Indices, BufferUsage.Static);

            vertexArray.SetIndexBuffer(indexBuffer);
            return vertexArray;
        }

        private static VertexArray CreateLineVertexArray()
        {
            VertexArray vertexArray = new OpenGLVertexArray();

            var buffer = new OpenGLVertexBuffer()
            {
                Layout = new VertexBufferLayout(new()
                {
                    new(VertexBufferElementType.Float3, "a_Position")
                })
            };
            vertexArray.AddVertexBuffer(buffer);

            return vertexArray;
        }
    }
}