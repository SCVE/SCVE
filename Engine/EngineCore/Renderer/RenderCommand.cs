using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public static class RenderCommand
    {
        public static void Init()
        {
            _rendererAPI.Init();
        }

        public static void SetViewport(uint x, uint y, uint width, uint height)
        {
            _rendererAPI.SetViewport(x, y, width, height);
        }

        public static void SetClearColor(Vector4 color)
        {
            _rendererAPI.SetClearColor(color);
        }

        public static void Clear()
        {
            _rendererAPI.Clear();
        }

        public static void DrawIndexed(VertexArray vertexArray, uint indexCount = 0)
        {
            _rendererAPI.DrawIndexed(vertexArray, indexCount);
        }

        private static RendererAPI _rendererAPI = RendererAPI.Create();
    }
}