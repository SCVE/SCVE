using System.Collections.Generic;
using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Components
{
    public class TexturedComponent : Component
    {
        private readonly VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;
        private readonly Texture _texture;
        
        public TexturedComponent()
        {
            _vertexArray = Application.Instance.RenderEntitiesCreator.CreateVertexArray();

            var buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(new[]
            {
                // Top left
                -0.5f, 0.5f, 0f, 0, 1,
                // Top right
                0.5f, 0.5f, 0f, 1, 1,
                // Bottom right
                0.5f, -0.5f, 0f, 1, 0,
                // Bottom left
                -0.5f, -0.5f, 0f, 0, 0,
            });


            buffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position"),
                new(VertexBufferElementType.Float2, "a_TextureCoordinate"),
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
            layout(location = 1) in vec2 a_TextureCoordinate;

            out vec3 v_Position;
            out vec2 v_TextureCoordinate;

            void main()
            {
                v_Position = a_Position;
                v_TextureCoordinate = a_TextureCoordinate;
                gl_Position = vec4(a_Position, 1.0);	
            }
            ";

            string fragmentSrc = @"
            #version 330 core
			
            layout(location = 0) out vec4 color;

            in vec3 v_Position;
            in vec2 v_TextureCoordinate;

            uniform sampler2D texture0;

            void main()
            {
                // color = vec4(v_Position, 1.0) ; 
                color = texture(texture0, v_TextureCoordinate);
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

            using (var textureData = Application.Instance.TextureLoader.Load("assets/Font/arial/atlas.png"))
            {
                _texture = Application.Instance.RenderEntitiesCreator.CreateTexture(textureData);
            }
        }

        public override void Render(IRenderer renderer)
        {
            _texture.Bind(0);
            
            _shaderProgram.Bind();
            renderer.RenderSolid(_vertexArray);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}