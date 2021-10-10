using OpenTK.Mathematics;

namespace Engine.EngineCore.Renderer
{
    public class Renderer
    {
        public static void Init()
        {
            RenderCommand.Init();
            // Renderer2D.Init();
        }

        public static void Shutdown()
        {
            // Renderer2D.Shutdown();
        }

        public static void OnWindowResize(uint width, uint height)
        {
            RenderCommand.SetViewport(0, 0, width, height);
        }

        public static void BeginScene(OrthographicCamera camera)
        {
            _sceneData.ViewProjectionMatrix = camera.GetViewProjectionMatrix();
        }

        public static void EndScene()
        {
        }

        public static void Submit(Shader shader, VertexArray vertexArray, Matrix4 transform)
        {
            shader.Bind();
            shader.SetMat4("u_ViewProjection", _sceneData.ViewProjectionMatrix);
            shader.SetMat4("u_Transform", transform);

            vertexArray.Bind();
            RenderCommand.DrawIndexed(vertexArray);
        }

        public static RendererAPI.API GetAPI()
        {
            return RendererAPI.GetAPI();
        }

        private struct SceneData
        {
            public Matrix4 ViewProjectionMatrix;
        };

        private static SceneData _sceneData = new SceneData();
    }
}