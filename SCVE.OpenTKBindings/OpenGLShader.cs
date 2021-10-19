using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLShader : Shader
    {
        public OpenGLShader(string source, ScveShaderType type)
        {
            Logger.Trace("Construction OpenGLShader");
            Id = GL.CreateShader(type.ToGLShaderType());
            GL.ShaderSource(Id, source);
        }

        public override void Compile()
        {
            Logger.Trace("OpenGLShader.Compile()");
            GL.CompileShader(Id);

            // Call this only when SPIRV is implemented
            // GL.SpecializeShader(Id, "main", 0, null, (int[])null);
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLShader.Dispose()");
            GL.DeleteShader(Id);
        }
    }
}