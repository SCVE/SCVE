using System.Collections.Generic;
using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class BasicComponent : IComponent
    {
        private VertexArray _vertexArray;
        private Program _program;
        
        public List<IComponent> Children { get; }

        public BasicComponent()
        {
            Children = new();
            
            _vertexArray = Application.Instance.RenderEntitiesCreator.CreateVertexArray();

            var buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(new[]
            {
                // Top left
                -0.5f, 0.5f, 0f, 1, 0, 0, 1,
                // Top right
                0.5f, 0.5f, 0f, 0, 1, 0, 1,
                // Bottom right
                0.5f, -0.5f, 0f, 0, 0, 1, 1,
                // Bottom left
                -0.5f, -0.5f, 0f, 1, 1, 1, 1,
            });

            // _buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(new[]
            // {
            //     -0.5f, -0.5f, 0.0f, 0.8f, 0.2f, 0.8f, 1.0f,
            //     0.5f, -0.5f, 0.0f, 0.2f, 0.3f, 0.8f, 1.0f,
            //     0.0f, 0.5f, 0.0f, 0.8f, 0.8f, 0.2f, 1.0f
            // });

            buffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position"),
                new(VertexBufferElementType.Float4, "a_Color"),
            });
            _vertexArray.AddVertexBuffer(buffer);

            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(new[]
            {
                0, 1, 2,
                2, 3, 0
            });

            _vertexArray.SetIndexBuffer(indexBuffer);

            string vertexSrc = @"
            #version 330 core
			
            layout(location = 0) in vec3 a_Position;
            layout(location = 1) in vec4 a_Color;

            out vec3 v_Position;
            out vec4 v_Color;

            void main()
            {
                v_Position = a_Position;
                v_Color = a_Color;
                gl_Position = vec4(a_Position, 1.0);	
            }
            ";

            string fragmentSrc = @"
            #version 330 core
			
            layout(location = 0) out vec4 color;

            in vec3 v_Position;
            in vec4 v_Color;

            void main()
            {
                // color = vec4(v_Position, 1.0);
                color = v_Color;
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

        public void Render(IRenderer renderer)
        {
            renderer.Render(_vertexArray, _program);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}