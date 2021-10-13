using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public class OpenGLShader : Shader
    {
        public OpenGLShader(string source, ScveShaderType type)
        {
            Id = GL.CreateShader(type.ToGLShaderType());
            GL.ShaderSource(Id, source);
        }

        public override void Compile()
        {
            GL.CompileShader(Id);
        }

        public override void Dispose()
        {
            GL.DeleteShader(Id);
        }
    }
}