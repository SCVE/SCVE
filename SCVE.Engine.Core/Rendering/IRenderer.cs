using SCVE.Engine.Core.Entities;
using SCVE.Engine.Core.Lifecycle;
using SCVE.Engine.Core.Primitives;
using SCVE.Engine.Core.Texts;

namespace SCVE.Engine.Core.Rendering
{
    public interface IRenderer : IInitable, ITerminatable
    {
        void Clear();

        void SetFromWindow(ScveWindow window);

        void RenderSolid(VertexArray vertexArray, ShaderProgram shaderProgram);

        void RenderWireframe(VertexArray vertexArray, ShaderProgram shaderProgram);
    }
}