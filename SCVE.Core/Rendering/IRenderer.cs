using SCVE.Core.Entities;
using SCVE.Core.Lifecycle;
using SCVE.Core.Primitives;

namespace SCVE.Core.Rendering
{
    public interface IRenderer : IInitable, ITerminatable
    {
        void Clear();

        void SetClearColor(ColorRgba colorRgba);

        void SetViewport(int x, int y, int width, int height);

        void RenderSolid(VertexArray vertexArray);
        
        void RenderWireframe(VertexArray vertexArray);
    }
}