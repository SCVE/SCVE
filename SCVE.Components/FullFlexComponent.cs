using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class FullFlexComponent : Component
    {
        private readonly VertexArray _vertexArray;
        private readonly Program _program;

        public FullFlexComponent(Rect rect) : base(rect)
        {
            _vertexArray = Application.Instance.RenderEntitiesCreator.CreateVertexArray();

            // ModelMatrix.Multiply(ScveMatrix4X4.CreateScale(2, 0.3f, 1f));

            var rectGeometry = GeometryGenerator.GenerateRect(Rect);

            var buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(rectGeometry.Vertices);

            buffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position")
            });
            _vertexArray.AddVertexBuffer(buffer);

            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(rectGeometry.Indices);

            _vertexArray.SetIndexBuffer(indexBuffer);

            _program = Application.Instance.ShaderProgramCache.LoadOrCache("FlatColor_MVP_Uniform");
        }

        public override void Render(IRenderer renderer)
        {
            _program.SetVector4("u_Color", 1, 1, 1, 1);
            
            // _program.SetMatrix4("u_MVP",
            //     ModelMatrix
            //         .MakeIdentity()
            //         .Multiply(Application.Instance.ViewProjectionAccessor.ViewProjectionMatrix)
            // );
            
            _program.SetMatrix4("u_Model",
                ModelMatrix
            );
            _program.SetMatrix4("u_View",
                Application.Instance.ViewProjectionAccessor.ViewMatrix
            );
            _program.SetMatrix4("u_Proj",
                Application.Instance.ViewProjectionAccessor.ProjectionMatrix
            );
            _program.Bind();

            renderer.RenderSolid(_vertexArray);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}