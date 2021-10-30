using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;

namespace SCVE.Components.Legacy
{
    public class TextViaAtlasComponent : RenderableComponent
    {
        public ScveFont Font { get; set; }

        private VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;

        private string _fontFileName;
        private float _fontSize;
        private string _text;

        public TextViaAtlasComponent(string fontFileName, float fontSize, string text)
        {
            Logger.Construct(nameof(TextViaAtlasComponent));
            _fontFileName = fontFileName;
            _fontSize = fontSize;
            _text = text;

            _shaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("Text");

            // NOTE: for text alignment we can precalculate the sum of all advances (width of the text)

            Application.Instance.Input.Scroll += InputOnScroll;

            Rebuild();
        }

        protected override void OnResized()
        {
            // We need to override this, because no scaling is needed for text
            var translation = TranslationMatrix.MakeTranslation(X, Y);
            ModelMatrix.MakeIdentity().Multiply(translation);
        }

        private void InputOnScroll(float arg1, float arg2)
        {
            Logger.Warn($"Scrolled {arg2}");
            _fontSize += arg2;
            Rebuild();
        }

        private void Rebuild()
        {
            Font = Application.Instance.Cache.Font.GetOrCache(_fontFileName, Maths.ClosestFontSizeUp(_fontSize));
            SetText(_text);
        }

        public void SetText(string text)
        {
            var cachedVertexArray = Application.Instance.Cache.VertexArray.Get($"Text({text}, {_fontSize})");

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
                float textureLeft = (float)chunk.TextureCoordX / Font.Texture.Width;
                float textureTop = 1 - (float)chunk.TextureCoordY / Font.Texture.Height;
                float textureRight = (float)(chunk.TextureCoordX + Font.Atlas.ChunkSize) / Font.Texture.Width;
                float textureBottom = 1 - (float)(chunk.TextureCoordY + Font.Atlas.ChunkSize) / Font.Texture.Height;

                // NOTE: When we overlap triangles with same Z and have a Z-Depth Test, OpenGL have a collision, and stops rendering them correctly
                // So I simply add a small Z offset to each character quad (some kind of stack), so OpenGL can sort the quads and render them correctly
                // Visible like this [[[[  ] instead of  [[[]]]
                // Take this into account when enabling DEPTH_TEST
                float zOffset = 0f; //i * 0.005f;

                // top left
                vertices[i * 12 + 0] = x;
                vertices[i * 12 + 1] = Font.Atlas.ChunkSize - chunk.BearingY;
                vertices[i * 12 + 2] = zOffset;

                textureCoordinates[i * 8 + 0] = textureLeft;
                textureCoordinates[i * 8 + 1] = textureTop;

                // top right
                vertices[i * 12 + 3] = x + Font.Atlas.ChunkSize;
                vertices[i * 12 + 4] = Font.Atlas.ChunkSize - chunk.BearingY;
                vertices[i * 12 + 5] = zOffset;

                textureCoordinates[i * 8 + 2] = textureRight;
                textureCoordinates[i * 8 + 3] = textureTop;

                // Bottom right
                vertices[i * 12 + 6] = x + Font.Atlas.ChunkSize;
                vertices[i * 12 + 7] = Font.Atlas.ChunkSize - chunk.BearingY + Font.Atlas.ChunkSize;
                vertices[i * 12 + 8] = zOffset;

                textureCoordinates[i * 8 + 4] = textureRight;
                textureCoordinates[i * 8 + 5] = textureBottom;

                // Bottom left
                vertices[i * 12 + 9] = x;
                vertices[i * 12 + 10] = Font.Atlas.ChunkSize - chunk.BearingY + Font.Atlas.ChunkSize;
                vertices[i * 12 + 11] = zOffset;

                textureCoordinates[i * 8 + 6] = textureLeft;
                textureCoordinates[i * 8 + 7] = textureBottom;

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
            var verticesVertexBuffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(vertices, BufferUsage.Static);
            verticesVertexBuffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position")
            });
            var textureCoordinatesVertexBuffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(textureCoordinates, BufferUsage.Static);
            textureCoordinatesVertexBuffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float2, "a_TextureCoordinate")
            });
            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(indices, BufferUsage.Static);
            _vertexArray.AddVertexBuffer(verticesVertexBuffer);
            _vertexArray.AddVertexBuffer(textureCoordinatesVertexBuffer);
            _vertexArray.SetIndexBuffer(indexBuffer);

            Application.Instance.Cache.VertexArray.AddOrReplace($"Text({text}, {_fontSize})", _vertexArray);
        }

        public override void Render(IRenderer renderer)
        {
            _shaderProgram.SetVector4("u_Color", 1, 1, 1, 1);
            Font.Texture.Bind(0);
            
            _shaderProgram.SetMatrix4("u_Model",
                ModelMatrix
            );
            _shaderProgram.SetMatrix4("u_View",
                renderer.GetViewMatrix()
            );
            _shaderProgram.SetMatrix4("u_Proj",
                renderer.GetProjectionMatrix()
            );
            _shaderProgram.Bind();

            renderer.RenderSolid(_vertexArray, _shaderProgram);

            //renderer.RenderText(Font, _text, _fontSize, X, Y, PixelWidth, PixelHeight);
        }
    }
}