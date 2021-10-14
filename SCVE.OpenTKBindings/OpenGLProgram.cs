using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class OpenGLProgram : Program
    {
        public OpenGLProgram()
        {
            Logger.Trace("Constructing OpenGLProgram");
            Id = GL.CreateProgram();
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

        public override void Link()
        {
            Logger.Trace("OpenGLProgram.Link()");
            GL.LinkProgram(Id);
        }

        public override void Dispose()
        {
            Logger.Trace("OpenGLProgram.Dispose()");
            GL.DeleteProgram(Id);
        }
    }
}