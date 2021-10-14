using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public class OpenGLRenderEntitiesCreator : IRenderEntitiesCreator
    {
        public VertexBuffer CreateVertexBuffer(float[] vertices)
        {
            return new OpenGLVertexBuffer(vertices);
        }

        public VertexBuffer CreateVertexBuffer(int size)
        {
            return new OpenGLVertexBuffer(size);
        }

        public VertexArray CreateVertexArray()
        {
            return new OpenGLVertexArray();
        }

        public IndexBuffer CreateIndexBuffer(int[] indices)
        {
            return new OpenGLIndexBuffer(indices);
        }

        public Shader CreateShader(string source, ScveShaderType type)
        {
            return new OpenGLShader(source, type);
        }

        public Program CreateProgram()
        {
            return new OpenGLProgram();
        }

        public Texture CreateTexture(TextureData textureData)
        {
            return new OpenGLTexture(textureData);
        }
    }
}