namespace SCVE.Core.Caches
{
    public class CachesContainer
    {
        public FontCache Font { get; set; } = new();

        public ShaderProgramCache ShaderProgram { get; set; } = new();

        public VertexArrayCache VertexArray { get; set; } = new();
    }
}