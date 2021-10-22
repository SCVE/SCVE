using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLShaderProgram : ShaderProgram
    {
        private Dictionary<string, int> _uniformLocations = new();

        public OpenGLShaderProgram()
        {
            Logger.Construct(nameof(OpenGLShaderProgram));
            Id = GL.CreateProgram();
        }

        public OpenGLShaderProgram(byte[] binary, int extension)
        {
            Logger.Construct(nameof(OpenGLShaderProgram));
            Id = GL.CreateProgram();

            GL.ProgramBinary(Id, (BinaryFormat)extension, binary, binary.Length);
        }

        private int GetOrCacheAttributeLocation(string name)
        {
            if (_uniformLocations.ContainsKey(name)) return _uniformLocations[name];

            var location = GL.GetUniformLocation(Id, name);
            _uniformLocations[name] = location;

            return _uniformLocations[name];
        }

        public override void AttachShader(Shader shader)
        {
            Logger.Trace("OpenGLProgram.AttachShader()");
            GL.AttachShader(Id, shader.Id);
        }

        public override void DetachShader(Shader shader)
        {
            Logger.Trace("OpenGLProgram.DetachShader()");
            GL.DetachShader(Id, shader.Id);
        }

        public override void Bind()
        {
            Logger.Trace("OpenGLProgram.Bind()");
            GL.UseProgram(Id);
        }

        public override void Unbind()
        {
            Logger.Trace("OpenGLProgram.Unbind()");
            GL.UseProgram(0);
        }

        public override void SetVector4(string name, float x, float y, float z, float w)
        {
            Bind();
            GL.Uniform4(GetOrCacheAttributeLocation(name), x, y, z, w);
        }

        public override void SetVector3(string name, float x, float y, float z)
        {
            Bind();
            GL.Uniform3(GetOrCacheAttributeLocation(name), x, y, z);
        }

        public override void SetMatrix4(string name, ScveMatrix4X4 matrix)
        {
            Bind();
            GL.UniformMatrix4(GetOrCacheAttributeLocation(name), 1, false, matrix.GetRawValues());
        }

        public override void SetFloat(string name, float value)
        {
            Bind();
            GL.Uniform1(GetOrCacheAttributeLocation(name), value);
        }

        public override void SetInt(string name, int value)
        {
            Bind();
            GL.Uniform1(GetOrCacheAttributeLocation(name), value);
        }

        public override void Link()
        {
            Logger.Trace("OpenGLProgram.Link()");
            GL.LinkProgram(Id);

            GL.GetProgram(Id, GetProgramParameterName.LinkStatus, out var isLinked);
            if (isLinked == 0)
            {
                GL.GetProgram(Id, GetProgramParameterName.InfoLogLength, out var maxLength);

                GL.GetProgramInfoLog(Id, maxLength, out var length, out var infoLog);

                Logger.Fatal("Shader linking failed ({0}):\n{1}", "", infoLog);
                Application.Instance.RequestTerminate();
            }
        }

        public override ShaderProgramBinaryData GetBinary()
        {
            // NOTE: All.ProgramBinaryLength is required because OpenTK lost this parameter
            GL.GetProgram(Id, (GetProgramParameterName)All.ProgramBinaryLength, out var binaryLength);

            byte[] buffer = new byte[binaryLength];
            GL.GetProgramBinary(Id, binaryLength, out var length, out var binaryFormat, buffer);

            return new ShaderProgramBinaryData(buffer, (int)binaryFormat);
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLProgram.Dispose()");
            GL.DeleteProgram(Id);
        }
    }
}