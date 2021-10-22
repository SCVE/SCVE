using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class RectComponent : Component
    {
        private readonly VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;
        private ColorRgba _colorRgba;

        public RectComponent(ColorRgba colorRgba)
        {
            _colorRgba = colorRgba;
            _vertexArray = Application.Instance.RenderEntitiesCreator.CreateVertexArray();

            var rectGeometry = GeometryGenerator.GenerateUnitSquare();

            var buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(rectGeometry.Vertices);

            buffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position")
            });
            _vertexArray.AddVertexBuffer(buffer);

            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(rectGeometry.Indices);

            _vertexArray.SetIndexBuffer(indexBuffer);

            string vertexSrc = @"
            #version 330 core
			
            layout(location = 0) in vec3 a_Position;

            out vec3 v_Position;

            void main()
            {
                v_Position = a_Position;
                gl_Position = vec4(a_Position, 1.0);	
            }
            ";

            string fragmentSrc = @"
            #version 330 core
			
            layout(location = 0) out vec4 color;

            in vec3 v_Position;

            uniform vec4 u_Color;

            void main()
            {
                color = u_Color;
            }
            ";

            using var vertexShader = Application.Instance.RenderEntitiesCreator.CreateShader(vertexSrc, ScveShaderType.Vertex);
            using var fragmentShader = Application.Instance.RenderEntitiesCreator.CreateShader(fragmentSrc, ScveShaderType.Fragment);

            vertexShader.Compile();
            fragmentShader.Compile();

            _shaderProgram = Application.Instance.RenderEntitiesCreator.CreateProgram();

            _shaderProgram.AttachShader(vertexShader);
            _shaderProgram.AttachShader(fragmentShader);
            _shaderProgram.Link();

            _shaderProgram.DetachShader(vertexShader);
            _shaderProgram.DetachShader(fragmentShader);
        }

        public override void Render(IRenderer renderer)
        {
            _shaderProgram.SetVector4("u_Color", _colorRgba.R, _colorRgba.G, _colorRgba.B, _colorRgba.A);

            _shaderProgram.Bind();
            renderer.RenderSolid(_vertexArray);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}