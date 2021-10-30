using SCVE.Core.Entities;
using SCVE.Core.Lifecycle;
using SCVE.Core.Primitives;
using SCVE.Core.Texts;

namespace SCVE.Core.Rendering
{
    public interface IRenderer : IInitable, ITerminatable
    {
        void Clear();

        void SetFromWindow(ScveWindow window);

        void SetClearColor(ColorRgba colorRgba);

        void SetViewport(int x, int y, int width, int height);

        ScveMatrix4X4 GetViewMatrix();
        ScveMatrix4X4 GetProjectionMatrix();

        void RenderSolid(VertexArray vertexArray, ShaderProgram shaderProgram);

        void RenderWireframe(VertexArray vertexArray, ShaderProgram shaderProgram);

        void RenderColorRect(float x, float y, float width, float height, ColorRgba colorRgba);

        void RenderLine(float x1, float y1, float x2, float y2, ColorRgba colorRgba, float width = 1f);

        void RenderText(ScveFont font, string text, float fontSize, float x, float y, ColorRgba color);

        void RenderText(ScveFont font, string text, float fontSize, float x, float y, ColorRgba color, float clipWidth, float clipHeight);
    }
}