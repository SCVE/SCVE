using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;

namespace SCVE.Core.Components
{
    public class BasicComponent : IRenderable
    {
        private VertexBuffer _buffer;
        public BasicComponent()
        {
            _buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(new[]
            {
                // Top left
                -0.5f, -0.5f, 0f,
                // Top right
                0.5f, -0.5f, 0f,
                // Bottom right
                0.5f, 0.5f, 0f,
                // Bottom left
                -0.5f, 0.5f, 0f,
            });
        }

        public void Render(IRenderer renderer)
        {
        }
    }
}