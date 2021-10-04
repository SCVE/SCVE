using System;
using System.Collections.Generic;
using Engine.EngineCore.Renderer;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Engine.Platform
{
    public class OpenGLVertexArray : VertexArray
    {
        public OpenGLVertexArray()
        {
            _rendererId = GL.CreateVertexArray();
        }

        ~OpenGLVertexArray()
        {
            GL.DeleteVertexArray(_rendererId);
        }

        private static VertexAttribPointerType ShaderDataTypeToOpenGLBaseType(ShaderDataType type)
        {
            if (type == ShaderDataType.Float ||
                type == ShaderDataType.Float2 ||
                type == ShaderDataType.Float3 ||
                type == ShaderDataType.Float4 ||
                type == ShaderDataType.Mat3 ||
                type == ShaderDataType.Mat4)
                return VertexAttribPointerType.Float;
            if (type == ShaderDataType.Int || 
                type == ShaderDataType.Int2 || 
                type == ShaderDataType.Int3 || 
                type == ShaderDataType.Int4)
                return VertexAttribPointerType.Int;
            if (type == ShaderDataType.Bool)
                return VertexAttribPointerType.Byte;

            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        public override void Bind()
        {
            GL.BindVertexArray(_rendererId);
        }

        public override void Unbind()
        {
            GL.BindVertexArray(VertexArrayHandle.Zero);
        }

        public override unsafe void AddVertexBuffer(VertexBuffer vertexBuffer)
        {
            var layout = vertexBuffer.GetLayout();
            if (layout.GetElements().Count == 0)
            {
                throw new ArgumentException("Vertex Buffer has no layout!");
            }

            GL.BindVertexArray(_rendererId);
            vertexBuffer.Bind();

            foreach (var element in layout.GetElements())
            {
                if (element.Type == ShaderDataType.Float ||
                    element.Type == ShaderDataType.Float2 ||
                    element.Type == ShaderDataType.Float3 ||
                    element.Type == ShaderDataType.Float4)
                {
                    GL.EnableVertexAttribArray(_vertexBufferIndex);
                    GL.VertexAttribPointer(
                        _vertexBufferIndex,
                        (int)element.GetComponentCount(),
                        ShaderDataTypeToOpenGLBaseType(element.Type),
                        element.Normalized ? (byte)1 : (byte)0,
                        (int)layout.GetStride(),
                        (void*)element.Offset);
                    _vertexBufferIndex++;
                }

                if (element.Type == ShaderDataType.Int ||
                    element.Type == ShaderDataType.Int2 ||
                    element.Type == ShaderDataType.Int3 ||
                    element.Type == ShaderDataType.Int4 ||
                    element.Type == ShaderDataType.Bool)
                {
                    GL.EnableVertexAttribArray(_vertexBufferIndex);
                    GL.VertexAttribIPointer(_vertexBufferIndex,
                        (int)element.GetComponentCount(),
                        (VertexAttribIType)ShaderDataTypeToOpenGLBaseType(element.Type),
                        (int)layout.GetStride(),
                        (void*)element.Offset);
                    _vertexBufferIndex++;
                    break;
                }

                if (element.Type == ShaderDataType.Mat3 ||
                    element.Type == ShaderDataType.Mat4)
                {
                    int count = (int)element.GetComponentCount();
                    for (byte i = 0; i < count; i++)
                    {
                        GL.EnableVertexAttribArray(_vertexBufferIndex);
                        GL.VertexAttribPointer(
                            _vertexBufferIndex,
                            count,
                            ShaderDataTypeToOpenGLBaseType(element.Type),
                            element.Normalized ? (byte)1 : (byte)0,
                            (int)layout.GetStride(),
                            (void*)(element.Offset + sizeof(float) * count * i));
                        GL.VertexAttribDivisor(_vertexBufferIndex, 1);
                        _vertexBufferIndex++;
                    }
                    break;
                }

                throw new ArgumentOutOfRangeException("Unknown ShaderDataType!");
            }
        }

        public override void SetIndexBuffer(IndexBuffer indexBuffer)
        {
        }

        public override IList<VertexBuffer> GetVertexBuffers()
        {
            return _vertexBuffers;
        }

        public override IndexBuffer GetIndexBuffer()
        {
            return _indexBuffer;
        }

        private VertexArrayHandle _rendererId;
        private uint _vertexBufferIndex = 0;
        private IList<VertexBuffer> _vertexBuffers;
        private IndexBuffer _indexBuffer;
    }
}