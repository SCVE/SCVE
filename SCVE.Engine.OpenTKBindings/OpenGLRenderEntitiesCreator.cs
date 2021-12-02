using SCVE.Engine.Core.Loading;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.OpenTKBindings
{
    public class OpenGLRenderEntitiesCreator : IRenderEntitiesCreator
    {
        public VertexBuffer CreateVertexBuffer(float[] vertices, BufferUsage usage)
        {
            return new OpenGLVertexBuffer(vertices, usage);
        }

        public VertexBuffer CreateVertexBuffer(int size, BufferUsage usage)
        {
            return new OpenGLVertexBuffer(size,  usage);
        }

        public VertexArray CreateVertexArray()
        {
            return new OpenGLVertexArray();
        }

        public IndexBuffer CreateIndexBuffer(int[] indices, BufferUsage usage)
        {
            return new OpenGLIndexBuffer(indices, usage);
        }

        public Shader CreateShader(string source, ScveShaderType type)
        {
            return new OpenGLShader(source, type);
        }

        public ShaderProgram CreateProgram()
        {
            return new OpenGLShaderProgram();
        }

        public ShaderProgram CreateProgram(byte[] binary, int extension)
        {
            return new OpenGLShaderProgram(binary, extension);
        }

        public Texture CreateTexture(TextureFileData textureFileData)
        {
            return new OpenGLTexture(textureFileData);
        }
    }
}