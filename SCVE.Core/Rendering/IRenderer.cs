using SCVE.Core.Entities;
using SCVE.Core.Lifecycle;

namespace SCVE.Core.Rendering
{
    public interface IRenderer : IInitable, ITerminatable
    {
        void Clear();

        void SetClearColor(Color color);

        void SetViewport(int x, int y, int width, int height);

        void Render(VertexArray vertexArray);
    }
}