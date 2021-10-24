using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class FullFlexWhiteRectComponentWhiteRect : Component
    {
        private readonly VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;

        public FullFlexWhiteRectComponentWhiteRect()
        {
            _vertexArray = Application.Instance.RenderEntitiesCreator.CreateVertexArray();

            ModelMatrix.Multiply(ScveMatrix4X4.CreateScale(200, 100, 1f));

            // var rectGeometry = GeometryGenerator.GenerateRect(Rect);
            var rectGeometry = GeometryGenerator.GenerateUnitSquare();

            var buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(rectGeometry.Vertices);

            buffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position")
            });
            _vertexArray.AddVertexBuffer(buffer);

            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(rectGeometry.Indices);

            _vertexArray.SetIndexBuffer(indexBuffer);

            _shaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("FlatColor_MVP_Uniform");
        }

        public override void Render(IRenderer renderer)
        {
            _shaderProgram.SetVector4("u_Color", 1, 1, 1, 1);
            
            _shaderProgram.SetMatrix4("u_Model",
                ModelMatrix
            );
            _shaderProgram.SetMatrix4("u_View",
                Application.Instance.ViewProjectionAccessor.ViewMatrix
            );
            _shaderProgram.SetMatrix4("u_Proj",
                Application.Instance.ViewProjectionAccessor.ProjectionMatrix
            );
            _shaderProgram.Bind();

            renderer.RenderSolid(_vertexArray);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}