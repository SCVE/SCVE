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

            string vertexSrc = @"
            #version 330 core
			
            layout(location = 0) in vec3 a_Position;

            // uniform mat4 u_MVP;

            uniform mat4 u_Model;
            uniform mat4 u_View;
            uniform mat4 u_Proj;

            out vec3 v_Position;

            void main()
            {
                v_Position = normalize(a_Position);
                // gl_Position = u_MVP * vec4(a_Position, 1.0);	
                gl_Position = u_Proj * u_View * u_Model * vec4(a_Position, 1.0);	
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

            _program = Application.Instance.RenderEntitiesCreator.CreateProgram();

            _program.AttachShader(vertexShader);
            _program.AttachShader(fragmentShader);
            _program.Link();

            _program.DetachShader(vertexShader);
            _program.DetachShader(fragmentShader);
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