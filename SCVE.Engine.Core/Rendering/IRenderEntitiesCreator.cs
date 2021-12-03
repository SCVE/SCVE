using SCVE.Engine.Core.Loading;

namespace SCVE.Engine.Core.Rendering
{
    public interface IRenderEntitiesCreator
    {
        VertexBuffer CreateVertexBuffer(float[] vertices, BufferUsage usage);

        VertexBuffer CreateVertexBuffer(int size, BufferUsage usage);

        VertexArray CreateVertexArray();

        IndexBuffer CreateIndexBuffer(int[] indices, BufferUsage usage);

        Shader CreateShader(string source, ScveShaderType type);

        ShaderProgram CreateProgram();

        ShaderProgram CreateProgram(byte[] binary, int extension);

        Texture CreateTexture(TextureFileData textureFileData);
    }
}