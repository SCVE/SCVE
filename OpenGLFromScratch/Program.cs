using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ErrorCode = OpenTK.Windowing.GraphicsLibraryFramework.ErrorCode;

namespace OpenGLFromScratch
{
    public static class Program
    {
        static unsafe void Main(string[] args)
        {
            GLFW.SetErrorCallback(ErrorCallback);

            if (!GLFW.Init())
            {
                GLFW.Terminate();
            }

            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 2);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 0);

            var window = GLFW.CreateWindow(640, 480, "Hello world", null, null);

            if (window is null)
            {
                GLFW.Terminate();
            }

            GLFW.SetKeyCallback(window, KeyCallback);
            GLFW.SetWindowSizeCallback(window, WindowSizeCallback);
            GLFW.SetWindowCloseCallback(window, CloseCallback);

            GLFW.MakeContextCurrent(window);

            GLFW.SwapInterval(1);

            GL.LoadBindings(new GLFWBindingsContext());

            Console.WriteLine("OpenGL Info");
            Console.WriteLine($"    Version: {GL.GetString(StringName.Version)}");
            Console.WriteLine($"    Renderer: {GL.GetString(StringName.Renderer)}");
            Console.WriteLine($"    Vendor: {GL.GetString(StringName.Vendor)}");
            Console.WriteLine($"    ShadingLanguageVersion: {GL.GetString(StringName.ShadingLanguageVersion)}");

            // var vertexArray = GL.GenVertexArray();
            //
            // GL.BindVertexArray(vertexArray);

            float[] gVertexBufferData =
            {
                -1.0f, -1.0f, 0.0f,
                1.0f, -1.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
            };

            var vertexBuffer = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * gVertexBufferData.Length, ref gVertexBufferData[0], BufferUsageHint.StaticDraw);

            var programHandle = LoadShaders("vertex_shader.glsl", "fragment_shader.glsl");

            while (!GLFW.WindowShouldClose(window))
            {
                GLFW.PollEvents();

                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.UseProgram(programHandle);

                GL.EnableVertexAttribArray(0);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

                GL.DisableVertexAttribArray(0);


                GLFW.SwapBuffers(window);
            }

            GLFW.DestroyWindow(window);

            GLFW.Terminate();
        }

        static unsafe int LoadShaders(string vertex_file_path, string fragment_file_path)
        {
            // Create the shaders
            int VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
            int FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);

            // Read the Vertex Shader code from the file
            string VertexShaderCode;
            if (File.Exists(vertex_file_path))
            {
                VertexShaderCode = File.ReadAllText(vertex_file_path);
            }
            else
            {
                Console.WriteLine("Impossible to open {0}. Are you in the right directory ? Don't forget to read the FAQ !", vertex_file_path);
                Console.ReadKey();
                return default;
            }

            // Read the Fragment Shader code from the file
            string FragmentShaderCode;
            if (File.Exists(fragment_file_path))
            {
                FragmentShaderCode = File.ReadAllText(fragment_file_path);
            }
            else
            {
                Console.WriteLine("Impossible to open {0}. Are you in the right directory ? Don't forget to read the FAQ !", fragment_file_path);
                Console.ReadKey();
                return default;
            }

            int Result = 0;
            int InfoLogLength;

            // Compile Vertex Shader
            Console.WriteLine("Compiling shader : {0}", vertex_file_path);
            GL.ShaderSource(VertexShaderID, VertexShaderCode);
            GL.CompileShader(VertexShaderID);

            // Check Vertex Shader
            GL.GetShader(VertexShaderID, ShaderParameter.CompileStatus, &Result);
            GL.GetShader(VertexShaderID, ShaderParameter.InfoLogLength, &InfoLogLength);
            if (InfoLogLength > 0)
            {
                GL.GetShaderInfoLog(VertexShaderID, InfoLogLength, null, out string VertexShaderErrorMessage);
                Console.WriteLine("{0}", VertexShaderErrorMessage);
            }

            // Compile Fragment Shader
            Console.WriteLine("Compiling shader : {0}", fragment_file_path);
            GL.ShaderSource(FragmentShaderID, FragmentShaderCode);
            GL.CompileShader(FragmentShaderID);

            // Check Fragment Shader
            GL.GetShader(FragmentShaderID, ShaderParameter.CompileStatus, &Result);
            GL.GetShader(FragmentShaderID, ShaderParameter.InfoLogLength, &InfoLogLength);
            if (InfoLogLength > 0)
            {
                GL.GetShaderInfoLog(VertexShaderID, InfoLogLength, null, out var FragmentShaderErrorMessage);
                Console.WriteLine("{0}", FragmentShaderErrorMessage);
            }

            // Link the program
            Console.WriteLine("Linking program");
            int ProgramID = GL.CreateProgram();
            GL.AttachShader(ProgramID, VertexShaderID);
            GL.AttachShader(ProgramID, FragmentShaderID);
            GL.LinkProgram(ProgramID);

            // Check the program
            GL.GetProgram(ProgramID, ProgramParameter.LinkStatus, &Result);
            GL.GetProgram(ProgramID, ProgramParameter.InfoLogLength, &InfoLogLength);
            if (InfoLogLength > 0)
            {
                GL.GetShaderInfoLog(VertexShaderID, InfoLogLength, null, out var ProgramErrorMessage);
                Console.WriteLine("{0}", ProgramErrorMessage);
            }

            GL.DetachShader(ProgramID, VertexShaderID);
            GL.DetachShader(ProgramID, FragmentShaderID);

            GL.DeleteShader(VertexShaderID);
            GL.DeleteShader(FragmentShaderID);

            return ProgramID;
        }


        private static unsafe void KeyCallback(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
        {
            if (key == Keys.Escape && action == InputAction.Press)
            {
                GLFW.SetWindowShouldClose(window, true);
            }
        }

        private static unsafe void CloseCallback(Window* window)
        {
            GLFW.SetWindowShouldClose(window, true);
        }

        private static unsafe void WindowSizeCallback(Window* window, int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        private static unsafe void ErrorCallback(ErrorCode error, string description)
        {
            Console.WriteLine($"{error} - {description}");
        }
    }
}