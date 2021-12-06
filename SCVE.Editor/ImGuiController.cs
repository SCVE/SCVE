using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Main;
using SCVE.Engine.Core.Primitives;
using SCVE.Engine.Core.Rendering;
using SCVE.Engine.OpenTKBindings;

namespace SCVE.Editor
{
    /// <summary>
    /// A modified version of Veldrid.ImGui's ImGuiRenderer.
    /// Manages input for ImGui and handles rendering ImGui's DrawLists with Veldrid.
    /// </summary>
    public class ImGuiController : IDisposable
    {
        private bool _frameBegun;

        private int _vertexArray;
        private int _vertexBuffer;
        private int _vertexBufferSize;
        private int _indexBuffer;
        private int _indexBufferSize;

        private Texture _fontTexture;
        private ShaderProgram _shader;

        private int _windowWidth;
        private int _windowHeight;

        public ImFontPtr arialFont;

        /// <summary>
        /// Constructs a new ImGuiController.
        /// </summary>
        public ImGuiController(int width, int height)
        {
            _windowWidth  = width;
            _windowHeight = height;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            var io = ImGui.GetIO();
            //io.Fonts.AddFontDefault();
            arialFont = io.Fonts.AddFontFromFileTTF("assets/Font/arial.ttf", 18);

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            CreateDeviceResources();
            SetKeyMappings();

            ImGui.StyleColorsDark();
            SetDarkTheme();

            SetPerFrameImGuiData(1f / 60f);

            ImGui.NewFrame();
            _frameBegun = true;
        }

        private void SetDarkTheme()
        {
            var colors = ImGui.GetStyle().Colors;
            colors[(int)ImGuiCol.WindowBg] = new System.Numerics.Vector4(0.1f, 0.105f, 0.11f, 1.0f);

            // Headers
            colors[(int)ImGuiCol.Header]        = new System.Numerics.Vector4(0.2f, 0.205f, 0.21f, 1.0f);
            colors[(int)ImGuiCol.HeaderHovered] = new System.Numerics.Vector4(0.3f, 0.305f, 0.31f, 1.0f);
            colors[(int)ImGuiCol.HeaderActive]  = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);

            // Buttons
            colors[(int)ImGuiCol.Button]        = new System.Numerics.Vector4(0.2f, 0.205f, 0.21f, 1.0f);
            colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0.3f, 0.305f, 0.31f, 1.0f);
            colors[(int)ImGuiCol.ButtonActive]  = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);

            // Frame BG
            colors[(int)ImGuiCol.FrameBg]        = new System.Numerics.Vector4(0.2f, 0.205f, 0.21f, 1.0f);
            colors[(int)ImGuiCol.FrameBgHovered] = new System.Numerics.Vector4(0.3f, 0.305f, 0.31f, 1.0f);
            colors[(int)ImGuiCol.FrameBgActive]  = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);

            // Tabs
            colors[(int)ImGuiCol.Tab]                = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);
            colors[(int)ImGuiCol.TabHovered]         = new System.Numerics.Vector4(0.38f, 0.3805f, 0.381f, 1.0f);
            colors[(int)ImGuiCol.TabActive]          = new System.Numerics.Vector4(0.28f, 0.2805f, 0.281f, 1.0f);
            colors[(int)ImGuiCol.TabUnfocused]       = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new System.Numerics.Vector4(0.2f, 0.205f, 0.21f, 1.0f);

            // Title
            colors[(int)ImGuiCol.TitleBg]          = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);
            colors[(int)ImGuiCol.TitleBgActive]    = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = new System.Numerics.Vector4(0.15f, 0.1505f, 0.151f, 1.0f);
        }

        public void WindowResized(int width, int height)
        {
            _windowWidth  = width;
            _windowHeight = height;
        }

        public void DestroyDeviceObjects()
        {
            Dispose();
        }

        public void CreateDeviceResources()
        {
            GL.CreateVertexArrays(1, out _vertexArray);

            _vertexBufferSize = 10000;
            _indexBufferSize  = 2000;
            GL.CreateBuffers(1, out _vertexBuffer);
            GL.CreateBuffers(1, out _indexBuffer);
            // Util.CreateVertexBuffer("ImGui", out _vertexBuffer);
            // Util.CreateElementBuffer("ImGui", out _indexBuffer);
            GL.NamedBufferData(_vertexBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.NamedBufferData(_indexBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            RecreateFontDeviceTexture();

            string VertexSource = @"#version 330 core

uniform mat4 projection_matrix;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;

out vec4 color;
out vec2 texCoord;

void main()
{
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}";
            string FragmentSource = @"#version 330 core

uniform sampler2D in_fontTexture;

in vec4 color;
in vec2 texCoord;

out vec4 outputColor;

void main()
{
    outputColor = color * texture(in_fontTexture, texCoord);
}";
            var vertexShader   = new OpenGLShader(VertexSource, ScveShaderType.Vertex);
            var fragmentShader = new OpenGLShader(FragmentSource, ScveShaderType.Fragment);
            _shader = new OpenGLShaderProgram();
            _shader.AttachShader(vertexShader);
            _shader.AttachShader(fragmentShader);
            _shader.Link();

            GL.VertexArrayVertexBuffer(_vertexArray, 0, _vertexBuffer, IntPtr.Zero, Unsafe.SizeOf<ImDrawVert>());
            GL.VertexArrayElementBuffer(_vertexArray, _indexBuffer);

            GL.EnableVertexArrayAttrib(_vertexArray, 0);
            GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 0, 2, VertexAttribType.Float, false, 0);

            GL.EnableVertexArrayAttrib(_vertexArray, 1);
            GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 1, 2, VertexAttribType.Float, false, 8);

            GL.EnableVertexArrayAttrib(_vertexArray, 2);
            GL.VertexArrayAttribBinding(_vertexArray, 2, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 2, 4, VertexAttribType.UnsignedByte, true, 16);

            // Util.CheckGLError("End of ImGui setup");
        }

        /// <summary>
        /// Recreates the device texture used to render text.
        /// </summary>
        public void RecreateFontDeviceTexture()
        {
            var io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out var width, out var height, out var bytesPerPixel);

            _fontTexture = new OpenGLTexture(width, height, pixels);
            // _fontTexture.SetMagFilter(TextureMagFilter.Linear);
            // _fontTexture.SetMinFilter(TextureMinFilter.Linear);

            io.Fonts.SetTexID((IntPtr)_fontTexture.Id);

            io.Fonts.ClearTexData();
        }

        /// <summary>
        /// Renders the ImGui draw list data.
        /// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
        /// or index data has increased beyond the capacity of the existing buffers.
        /// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
        /// </summary>
        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;
                ImGui.Render();
                RenderImDrawData(ImGui.GetDrawData());
            }
        }

        /// <summary>
        /// Updates ImGui input and IO configuration state.
        /// </summary>
        public void Update(float deltaSeconds)
        {
            if (_frameBegun)
            {
                ImGui.Render();
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput();

            _frameBegun = true;
            ImGui.NewFrame();
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            io.DisplaySize.X = _windowWidth;
            io.DisplaySize.Y = _windowHeight;

            // TODO: Apply DPI here
            io.DisplayFramebufferScale.X = 1;
            io.DisplayFramebufferScale.Y = 1;

            io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
        }

        readonly List<char> PressedChars = new List<char>();

        private void UpdateImGuiInput()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            var MouseState    = ScveEngine.Instance.Input.MouseState;
            var KeyboardState = ScveEngine.Instance.Input.KeyboardState;

            io.MouseDown[0] = MouseState[MouseCode.Left];
            io.MouseDown[1] = MouseState[MouseCode.Right];
            io.MouseDown[2] = MouseState[MouseCode.Middle];

            // var screenPoint = new Vector2i((int)ScveEngine.Instance.Input.GetCursorX(), (int)ScveEngine.Instance.Input.GetCursorY());
            // var point       = screenPoint; //wnd.PointToClient(screenPoint);
            io.MousePos.X = ScveEngine.Instance.Input.GetCursorX();
            io.MousePos.Y = ScveEngine.Instance.Input.GetCursorY();

            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (key == KeyCode.Unknown)
                {
                    continue;
                }

                io.KeysDown[(int)key] = KeyboardState[key];
            }

            foreach (var c in PressedChars)
            {
                io.AddInputCharacter(c);
            }

            PressedChars.Clear();

            io.KeyCtrl  = KeyboardState[KeyCode.LeftControl] || KeyboardState[KeyCode.RightControl];
            io.KeyAlt   = KeyboardState[KeyCode.LeftAlt] || KeyboardState[KeyCode.RightAlt];
            io.KeyShift = KeyboardState[KeyCode.LeftShift] || KeyboardState[KeyCode.RightShift];
            io.KeySuper = KeyboardState[KeyCode.LeftSuper] || KeyboardState[KeyCode.RightSuper];
        }

        internal void PressChar(char keyChar)
        {
            PressedChars.Add(keyChar);
        }

        internal void MouseScroll(float x, float y)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            io.MouseWheel  = y;
            io.MouseWheelH = x;
        }

        private static void SetKeyMappings()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab]        = (int)KeyCode.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow]  = (int)KeyCode.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)KeyCode.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow]    = (int)KeyCode.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow]  = (int)KeyCode.Down;
            io.KeyMap[(int)ImGuiKey.PageUp]     = (int)KeyCode.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown]   = (int)KeyCode.PageDown;
            io.KeyMap[(int)ImGuiKey.Home]       = (int)KeyCode.Home;
            io.KeyMap[(int)ImGuiKey.End]        = (int)KeyCode.End;
            io.KeyMap[(int)ImGuiKey.Delete]     = (int)KeyCode.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace]  = (int)KeyCode.Backspace;
            io.KeyMap[(int)ImGuiKey.Enter]      = (int)KeyCode.Enter;
            io.KeyMap[(int)ImGuiKey.Escape]     = (int)KeyCode.Escape;
            io.KeyMap[(int)ImGuiKey.A]          = (int)KeyCode.A;
            io.KeyMap[(int)ImGuiKey.C]          = (int)KeyCode.C;
            io.KeyMap[(int)ImGuiKey.V]          = (int)KeyCode.V;
            io.KeyMap[(int)ImGuiKey.X]          = (int)KeyCode.X;
            io.KeyMap[(int)ImGuiKey.Y]          = (int)KeyCode.Y;
            io.KeyMap[(int)ImGuiKey.Z]          = (int)KeyCode.Z;
        }

        private ScveMatrix4X4 _mvpMatrix = ScveMatrix4X4.Identity;

        private void RenderImDrawData(ImDrawDataPtr draw_data)
        {
            if (draw_data.CmdListsCount == 0)
            {
                return;
            }

            for (int i = 0; i < draw_data.CmdListsCount; i++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[i];

                int vertexSize = cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>();
                if (vertexSize > _vertexBufferSize)
                {
                    int newSize = (int)Math.Max(_vertexBufferSize * 1.5f, vertexSize);
                    GL.NamedBufferData(_vertexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    _vertexBufferSize = newSize;

                    Console.WriteLine($"Resized dear imgui vertex buffer to new size {_vertexBufferSize}");
                }

                int indexSize = cmd_list.IdxBuffer.Size * sizeof(ushort);
                if (indexSize > _indexBufferSize)
                {
                    int newSize = (int)Math.Max(_indexBufferSize * 1.5f, indexSize);
                    GL.NamedBufferData(_indexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    _indexBufferSize = newSize;

                    Console.WriteLine($"Resized dear imgui index buffer to new size {_indexBufferSize}");
                }
            }

            // Setup orthographic projection matrix into our constant buffer
            ImGuiIOPtr io = ImGui.GetIO();
            _mvpMatrix.MakeIdentity().MakeOrthographicOffCenter(
                0.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            _shader.Bind();
            _shader.SetMatrix4("projection_matrix", _mvpMatrix);
            _shader.SetInt("in_fontTexture", 0);
            // Util.CheckGLError("Projection");

            GL.BindVertexArray(_vertexArray);
            // Util.CheckGLError("VAO");

            draw_data.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            // Render command lists
            for (int n = 0; n < draw_data.CmdListsCount; n++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[n];

                GL.NamedBufferSubData(_vertexBuffer, IntPtr.Zero, cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd_list.VtxBuffer.Data);
                // Util.CheckGLError($"Data Vert {n}");

                GL.NamedBufferSubData(_indexBuffer, IntPtr.Zero, cmd_list.IdxBuffer.Size * sizeof(ushort), cmd_list.IdxBuffer.Data);
                // Util.CheckGLError($"Data Idx {n}");

                int vtx_offset = 0;
                int idx_offset = 0;

                for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);
                        // Util.CheckGLError("Texture");

                        // We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
                        var clip = pcmd.ClipRect;
                        GL.Scissor((int)clip.X, _windowHeight - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));
                        // Util.CheckGLError("Scissor");

                        if ((io.BackendFlags & ImGuiBackendFlags.RendererHasVtxOffset) != 0)
                        {
                            GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(idx_offset * sizeof(ushort)), vtx_offset);
                        }
                        else
                        {
                            GL.DrawElements(BeginMode.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (int)pcmd.IdxOffset * sizeof(ushort));
                        }
                        // Util.CheckGLError("Draw");
                    }

                    idx_offset += (int)pcmd.ElemCount;
                }

                vtx_offset += cmd_list.VtxBuffer.Size;
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.ScissorTest);
        }

        /// <summary>
        /// Frees all graphics resources used by the renderer.
        /// </summary>
        public void Dispose()
        {
            _fontTexture.Dispose();
            _shader.Dispose();
        }
    }
}