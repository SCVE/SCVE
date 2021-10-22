﻿using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Texts;

namespace SCVE.Components
{
    public class TextViaAtlasComponent : Component
    {
        public FontAtlasData AtlasData { get; set; }

        private VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;
        private readonly Texture _texture;

        public TextViaAtlasComponent()
        {
            Application.Instance.FontAtlasGenerator.Generate("arial.ttf", Alphabets.Default, 100);
            // using var textureData = Application.Instance.TextureLoader.Load("assets/texture_squares_1024_1024.png");
            using var textureData = Application.Instance.TextureLoader.Load("assets/Font/arial/atlas.png");

            _texture = Application.Instance.RenderEntitiesCreator.CreateTexture(textureData);

            AtlasData = Application.Instance.FileLoaders.FontAtlasData.Load("arial.ttf");

            _shaderProgram = Application.Instance.ShaderProgramCache.LoadOrCache("Text");

            SetText("Bird Egop is Super Cool");

            ModelMatrix.MakeIdentity().Multiply(ScveMatrix4X4.CreateTranslation(50, 50));
        }

        public void SetText(string text)
        {
            float x = 0f;

            float[] vertices = new float[text.Length * 4 * 3];
            int[] indices = new int[text.Length * 6];
            float[] textureCoordinates = new float[text.Length * 4 * 2];

            for (var i = 0; i < text.Length; i++)
            {
                var chunk = AtlasData.Chunks[(int)text[i]];

                float textureLeft = (float)chunk.TextureX / _texture.Width;
                float textureTop = 1 - (float)chunk.TextureY / _texture.Height;
                float textureRight = (float)(chunk.TextureX + AtlasData.ChunkWidth) / _texture.Width;
                float textureBottom = 1 - (float)(chunk.TextureY + AtlasData.ChunkWidth) / _texture.Height;

                // top left
                vertices[i * 12 + 0] = x;
                vertices[i * 12 + 1] = AtlasData.ChunkWidth - chunk.BearingY;
                vertices[i * 12 + 2] = i * 0.005f;

                textureCoordinates[i * 8 + 0] = textureLeft;
                textureCoordinates[i * 8 + 1] = textureTop;

                // textureCoordinates[i * 8 + 0] = 0;
                // textureCoordinates[i * 8 + 1] = 1;

                // top right
                vertices[i * 12 + 3] = x + AtlasData.ChunkWidth;
                vertices[i * 12 + 4] = AtlasData.ChunkWidth - chunk.BearingY;
                vertices[i * 12 + 5] = i * 0.005f;

                textureCoordinates[i * 8 + 2] = textureRight;
                textureCoordinates[i * 8 + 3] = textureTop;

                // textureCoordinates[i * 8 + 2] = 1;
                // textureCoordinates[i * 8 + 3] = 1;

                // Bottom right
                vertices[i * 12 + 6] = x + AtlasData.ChunkWidth;
                vertices[i * 12 + 7] = AtlasData.ChunkWidth - chunk.BearingY + AtlasData.ChunkWidth;
                vertices[i * 12 + 8] = i * 0.005f;

                textureCoordinates[i * 8 + 4] = textureRight;
                textureCoordinates[i * 8 + 5] = textureBottom;

                // textureCoordinates[i * 8 + 4] = 1;
                // textureCoordinates[i * 8 + 5] = 0;

                // Bottom left
                vertices[i * 12 + 9] = x;
                vertices[i * 12 + 10] = AtlasData.ChunkWidth - chunk.BearingY + AtlasData.ChunkWidth;
                vertices[i * 12 + 11] = i * 0.005f;

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
        }

        public override void Render(IRenderer renderer)
        {
            // _shaderProgram.SetVector4("u_Color", 1, 1, 1, 1);
            _texture.Bind(0);

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