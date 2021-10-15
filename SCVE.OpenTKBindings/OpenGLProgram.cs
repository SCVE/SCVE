using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLProgram : Program
    {
        private Dictionary<string, int> _uniformLocations = new();
        public OpenGLProgram()
        {
            Logger.Trace("Constructing OpenGLProgram");
            Id = GL.CreateProgram();
        }

        private int GetOrCacheAttributeLocation(string name)
        {
            if (!_uniformLocations.ContainsKey(name))
            {
                var location = GL.GetUniformLocation(Id, name);
                _uniformLocations[name] = location;
            }

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
            }
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLProgram.Dispose()");
            GL.DeleteProgram(Id);
        }
    }
}