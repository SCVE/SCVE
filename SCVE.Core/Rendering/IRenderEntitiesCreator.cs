namespace SCVE.Core.Rendering
{
    public interface IRenderEntitiesCreator
    {
        VertexBuffer CreateVertexBuffer(float[] vertices);
        VertexBuffer CreateVertexBuffer(int size);
        
        IndexBuffer CreateIndexBuffer(int[] indices);
        
        Shader CreateShader(string source, ScveShaderType type);
    }
}