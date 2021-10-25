using SCVE.Core.Loading;

namespace SCVE.Core.Rendering
{
    public interface IRenderEntitiesCreator
    {
        VertexBuffer CreateVertexBuffer(float[] vertices);

        VertexBuffer CreateVertexBuffer(int size);

        VertexArray CreateVertexArray();

        IndexBuffer CreateIndexBuffer(int[] indices);

        Shader CreateShader(string source, ScveShaderType type);

        ShaderProgram CreateProgram();

        ShaderProgram CreateProgram(byte[] binary, int extension);

        Texture CreateTexture(TextureFileData textureFileData);
    }
}