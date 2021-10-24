using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Loading;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class TextViaAtlasComponent : Component
    {
        public ScveFont Font { get; set; }

        private VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;

        private float _currentFontSize = 72;

        public TextViaAtlasComponent()
        {
            _shaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("Text");

            // NOTE: for text alignment we can precalculate the sum of all advances (width of the text)
            
            ModelMatrix.MakeIdentity()
                // .Multiply(ScveMatrix4X4.CreateScale(0.14f, 0.14f))
                .Multiply(ScveMatrix4X4.CreateTranslation(50, 50));
            
            Application.Instance.Input.Scroll += InputOnScroll;

            Init();
        }

        private void InputOnScroll(float arg1, float arg2)
        {
            Logger.Warn($"Scrolled {arg2}");
            _currentFontSize += arg2;
            Init();
        }

        private void Init()
        {
            Font = Application.Instance.Cache.Font.GetOrCache("arial.ttf", _currentFontSize);
            SetText("Bird Egop is Super Cool");
        }

        public void SetText(string text)
        {
            var cachedVertexArray = Application.Instance.Cache.VertexArray.Get($"Text({text}, {_currentFontSize})");

            if (cachedVertexArray is not null)
            {
                _vertexArray = cachedVertexArray;
                return;
            }
            
            float x = 0f;

            // We have 4 vertices for each character (quad) of 3 floats each
            float[] vertices = new float[text.Length * 4 * 3];
            
            // We have 2 triangles in each character (quad) of 3 vertices each
            int[] indices = new int[text.Length * 2 * 3];
            
            // We have 4 texture coordinate for each character (quad) of 2 floats each
            float[] textureCoordinates = new float[text.Length * 4 * 2];

            for (var i = 0; i < text.Length; i++)
            {
                var chunk = Font.Atlas.Chunks[(int)text[i]];

                // NOTE: we invert the Y axis, because Textures have (0,0) at top left, but OpenGL have (0,0) at bottom left
                float textureLeft = (float)chunk.TextureX / Font.Texture.Width;
                float textureTop = 1 - (float)chunk.TextureY / Font.Texture.Height;
                float textureRight = (float)(chunk.TextureX + Font.Atlas.ChunkSize) / Font.Texture.Width;
                float textureBottom = 1 - (float)(chunk.TextureY + Font.Atlas.ChunkSize) / Font.Texture.Height;

                // NOTE: When we overlap trianles with same Z, OpenGL have a collision, and stops rendering them correctly
                // So I simply add a small Z offset to each character quad (some kind of stack), so OpenGL can sort the quads and render them correctly
                // Visible like this [[[[  ] instead of  [[[]]]
                float zOffset = i * 0.005f;
                
                // top left
                vertices[i * 12 + 0] = x;
                vertices[i * 12 + 1] = Font.Atlas.ChunkSize - chunk.BearingY;
                vertices[i * 12 + 2] = zOffset;

                textureCoordinates[i * 8 + 0] = textureLeft;
                textureCoordinates[i * 8 + 1] = textureTop;

                // textureCoordinates[i * 8 + 0] = 0;
                // textureCoordinates[i * 8 + 1] = 1;

                // top right
                vertices[i * 12 + 3] = x + Font.Atlas.ChunkSize;
                vertices[i * 12 + 4] = Font.Atlas.ChunkSize - chunk.BearingY;
                vertices[i * 12 + 5] = zOffset;

                textureCoordinates[i * 8 + 2] = textureRight;
                textureCoordinates[i * 8 + 3] = textureTop;

                // textureCoordinates[i * 8 + 2] = 1;
                // textureCoordinates[i * 8 + 3] = 1;

                // Bottom right
                vertices[i * 12 + 6] = x + Font.Atlas.ChunkSize;
                vertices[i * 12 + 7] = Font.Atlas.ChunkSize - chunk.BearingY + Font.Atlas.ChunkSize;
                vertices[i * 12 + 8] = zOffset;

                textureCoordinates[i * 8 + 4] = textureRight;
                textureCoordinates[i * 8 + 5] = textureBottom;

                // textureCoordinates[i * 8 + 4] = 1;
                // textureCoordinates[i * 8 + 5] = 0;

                // Bottom left
                vertices[i * 12 + 9] = x;
                vertices[i * 12 + 10] = Font.Atlas.ChunkSize - chunk.BearingY + Font.Atlas.ChunkSize;
                vertices[i * 12 + 11] = zOffset;

                textureCoordinates[i * 8 + 6] = textureLeft;
                textureCoordinates[i * 8 + 7] = textureBottom;

                // textureCoordinates[i * 8 + 6] = 0;
                // textureCoordinates[i * 8 + 7] = 0;

                indices[i * 6 + 0] = i * 4 + 0;
                indices[i * 6 + 1] = i * 4 + 3;
                indices[i * 6 + 2] = i * 4 + 1;
                indices[i * 6 + 3] = i * 4 + 1;
                indices[i * 6 + 4] = i * 4 + 3;
                indices[i * 6 + 5] = i * 4 + 2;

                x += chunk.Advance;
                //x += AtlasData.ChunkWidth;
            }

            _vertexArray = Application.Instance.RenderEntitiesCreator.CreateVertexArray();
            var verticesVertexBuffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(vertices);
            verticesVertexBuffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position")
            });
            var textureCoordinatesVertexBuffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(textureCoordinates);
            textureCoordinatesVertexBuffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float2, "a_TextureCoordinate")
            });
            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(indices);
            _vertexArray.AddVertexBuffer(verticesVertexBuffer);
            _vertexArray.AddVertexBuffer(textureCoordinatesVertexBuffer);
            _vertexArray.SetIndexBuffer(indexBuffer);
            
            Application.Instance.Cache.VertexArray.AddOrReplace($"Text({text}, {_currentFontSize})", _vertexArray);
        }

        public override void Render(IRenderer renderer)
        {
            _shaderProgram.SetVector4("u_Color", 1, 1, 1, 1);
            Font.Texture.Bind(0);

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