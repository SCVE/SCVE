using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Engine.Platform.OpenGL
{
    public class OpenGLShader : Shader
    {
        public OpenGLShader(string filepath)
        {
            _filepath = filepath;
            Utils.CreateCacheDirectoryIfNeeded();

            string source = ReadFile(filepath);
            var shaderSources = PreProcess(source);

            {
                Stopwatch timer = Stopwatch.StartNew();
                CompileOrGetVulkanBinaries(shaderSources);
                CompileOrGetOpenGLBinaries();
                CreateProgram();
                Console.WriteLine("WARN: Shader creation took {0} ms", timer.ElapsedMilliseconds);
            }

            // Extract name from filepath
            var lastSlash = filepath.LastIndexOf("/\\");
            lastSlash = lastSlash == -1 ? 0 : lastSlash + 1;
            var lastDot = filepath.LastIndexOf('.');
            var count = lastDot == -1 ? filepath.Length - lastSlash : lastDot - lastSlash;
            _name = filepath.Substring(lastSlash, count);
        }

        public OpenGLShader(string name, string vertexSrc, string fragmentSrc)
        {
            Dictionary<ShaderType, string> sources = new();
            sources[ShaderType.VertexShader] = vertexSrc;
            sources[ShaderType.FragmentShader] = fragmentSrc;

            CompileOrGetVulkanBinaries(sources);
            CompileOrGetOpenGLBinaries();
            CreateProgram();
        }

        ~OpenGLShader()
        {
            GL.DeleteProgram(_rendererId);
        }

        public override void Bind()
        {
            GL.UseProgram(_rendererId);
        }

        public override void Unbind()
        {
            GL.UseProgram(ProgramHandle.Zero);
        }

        public override void SetInt(string name, int value)
        {
            UploadUniformInt(name, value);
        }

        public override unsafe void SetIntArray(string name, int* values, uint count)
        {
            UploadUniformIntArray(name, values, count);
        }

        public override void SetFloat(string name, float value)
        {
            UploadUniformFloat(name, value);
        }

        public override void SetFloat2(string name, Vector2 value)
        {
            UploadUniformFloat2(name, value);
        }

        public override void SetFloat3(string name, Vector3 value)
        {
            UploadUniformFloat3(name, value);
        }

        public override void SetFloat4(string name, Vector4 value)
        {
            UploadUniformFloat4(name, value);
        }

        public override void SetMat4(string name, Matrix4 value)
        {
            UploadUniformMat4(name, value);
        }

        public override string GetName()
        {
            return _name;
        }

        public void UploadUniformInt(string name, int value)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.Uniform1i(location, value);
        }

        public unsafe void UploadUniformIntArray(string name, int* values, uint count)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.Uniform1iv(location, (int)count, values);
        }

        public void UploadUniformFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.Uniform1f(location, value);
        }

        public void UploadUniformFloat2(string name, Vector2 value)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.Uniform2f(location, value.X, value.Y);
        }

        public void UploadUniformFloat3(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.Uniform3f(location, value.X, value.Y, value.Z);
        }

        public void UploadUniformFloat4(string name, Vector4 value)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.Uniform4f(location, value.X, value.Y, value.Z, value.W);
        }

        public unsafe void UploadUniformMat3(string name, Matrix3 matrix)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.UniformMatrix3fv(location, 1, 0, &matrix.Row0.X);
        }

        public unsafe void UploadUniformMat4(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_rendererId, name);
            GL.UniformMatrix4fv(location, 1, 0, &matrix.Row0.X);
        }

        private string ReadFile(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        private Dictionary<ShaderType, string> PreProcess(string source)
        {
            Dictionary<ShaderType, string> shaderSources = new();

            string typeToken = "#type";
            int typeTokenLength = typeToken.Length;
            int pos = source.IndexOf(typeToken, 0); //Start of shader type declaration line
            while (pos != -1)
            {
                int eol = source.IndexOf("\r\n", pos); //End of shader type declaration line
                if (eol == -1)
                {
                    throw new Exception("Syntax error");
                }

                int begin = pos + typeTokenLength + 1; //Start of shader type name (after "#type " keyword)
                string type = source.Substring(begin, eol - begin);

                // This will throw an exception inside
                // HZ_CORE_ASSERT(Utils.ShaderTypeFromString(type), "Invalid shader type specified");

                int nextLinePos = source.IndexOf("\r\n", eol); //Start of shader code after shader type declaration line
                if (nextLinePos == -1)
                {
                    throw new Exception("Syntax error");
                }

                pos = source.IndexOf(typeToken, nextLinePos + 1); //Start of next shader type declaration line

                shaderSources[Utils.ShaderTypeFromString(type)] = (pos == -1) ? source.Substring(nextLinePos) : source.Substring(nextLinePos, pos - nextLinePos);
            }

            return shaderSources;
        }

        private void CompileOrGetVulkanBinaries(Dictionary<ShaderType, string> shaderSources)
        {
        }

        private void CompileOrGetOpenGLBinaries()
        {
        }

        private unsafe void CreateProgram()
        {
            var program = GL.CreateProgram();

            List<ShaderHandle> shaderIDs = new();
            foreach (var (stage, spirv) in _openGLspirv)
            {
                ShaderHandle shaderID = GL.CreateShader(stage);
                shaderIDs.Add(shaderID);

                fixed (uint* spirvPtr = spirv)
                {
                    GL.ShaderBinary(1, &shaderID, ShaderBinaryFormat.ShaderBinaryFormatSpirV, (byte*)spirvPtr, spirv.Length * sizeof(uint));
                }


                GL.SpecializeShader(shaderID, "main", 0, 0, 0);
                GL.AttachShader(program, shaderID);
            }

            GL.LinkProgram(program);

            int isLinked = 0;
            GL.GetProgramiv(program, ProgramPropertyARB.LinkStatus, &isLinked);
            if (isLinked == 0)
            {
                int maxLength = 0;
                GL.GetProgramiv(program, ProgramPropertyARB.InfoLogLength, &maxLength);

                byte[] infoLog = new byte[maxLength];
                fixed (byte* infoLogPtr = infoLog)
                    GL.GetProgramInfoLog(program, maxLength, &maxLength, infoLogPtr);
                Console.WriteLine("ERROR: Shader linking failed ({0}):\n{1}", _filepath, Encoding.UTF8.GetString(infoLog));

                GL.DeleteProgram(program);

                foreach (var id in shaderIDs)
                    GL.DeleteShader(id);
            }

            foreach (var id in shaderIDs)
            {
                GL.DetachShader(program, id);
                GL.DeleteShader(id);
            }

            _rendererId = program;
        }

        private void Reflect(ShaderType stage, List<uint> shaderData)
        {
        }

        private ProgramHandle _rendererId;
        private string _filepath;
        private string _name;

        private Dictionary<ShaderType, uint[]> _vulkanSpirv;
        private Dictionary<ShaderType, uint[]> _openGLspirv;

        private Dictionary<ShaderType, string> _openGLSourceCode = new();
    }
}