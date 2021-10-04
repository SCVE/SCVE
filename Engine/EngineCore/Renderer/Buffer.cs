using System;
using System.Collections.Generic;

namespace Engine.EngineCore.Renderer
{
    public class ShaderDataType
    {
        public static readonly ShaderDataType None = 0;
        public static readonly ShaderDataType Float = 4;
        public static readonly ShaderDataType Float2 = 4 * 2;
        public static readonly ShaderDataType Float3 = 4 * 3;
        public static readonly ShaderDataType Float4 = 4 * 4;
        public static readonly ShaderDataType Mat3 = 4 * 3 * 3;
        public static readonly ShaderDataType Mat4 = 4 * 4 * 4;
        public static readonly ShaderDataType Int = 4;
        public static readonly ShaderDataType Int2 = 4 * 2;
        public static readonly ShaderDataType Int3 = 4 * 3;
        public static readonly ShaderDataType Int4 = 4 * 4;
        public static readonly ShaderDataType Bool = 1;

        public uint Value;

        public ShaderDataType(uint value)
        {
            Value = value;
        }

        public static implicit operator ShaderDataType(uint val)
        {
            return new(val);
        }

        public static implicit operator uint(ShaderDataType val)
        {
            return val.Value;
        }
        
        public static bool operator==(ShaderDataType left, ShaderDataType right)
        {
            return left.Value == right.Value;
        }

        public static bool operator!=(ShaderDataType left, ShaderDataType right)
        {
            return !(left == right);
        }
    }

    public struct BufferElement
    {
        public string Name;
        public ShaderDataType Type;
        public uint Size;
        public uint Offset;
        public bool Normalized;

        public BufferElement(string name, ShaderDataType type, uint offset, bool normalized)
        {
            Name = name;
            Type = type;
            Size = (uint)type;
            Offset = offset;
            Normalized = normalized;
        }

        public uint GetComponentCount()
        {
            return Type switch
            {
                ShaderDataType.Float or ShaderDataType.Int or ShaderDataType.Bool => 1,
                ShaderDataType.Float2 or ShaderDataType.Int2 => 2,
                ShaderDataType.Float3 or ShaderDataType.Mat3 or ShaderDataType.Int3 => 3,
                ShaderDataType.Float4 or ShaderDataType.Int4 or ShaderDataType.Mat4 => 4,
                _ => 0
            };
        }
    }

    public class BufferLayout
    {
        public BufferLayout()
        {
        }

        public BufferLayout(IList<BufferElement> elements)
        {
            _elements = elements;
            CalculateOffsetsAndStride();
        }

        public uint GetStride()
        {
            return _stride;
        }

        public IList<BufferElement> GetElements()
        {
            return _elements;
        }

        private void CalculateOffsetsAndStride()
        {
            uint offset = 0;
            _stride = 0;
            for (var index = 0; index < _elements.Count; index++)
            {
                var element = _elements[index];
                element.Offset = offset;
                offset += element.Size;
                _stride += element.Size;
            }
        }

        private IList<BufferElement> _elements;
        private uint _stride = 0;
    }

    public abstract class VertexBuffer
    {
        public abstract void Bind();

        public abstract void Unbind();

        public abstract unsafe void SetData(void* data, uint size);

        public abstract BufferLayout GetLayout();

        public abstract void SetLayout(BufferLayout layout);

        public static VertexBuffer Create(uint size)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None:    throw new ArgumentException("RendererAPI::None is currently not supported!"); 
                case RendererAPI.API.OpenGL:  return new OpenGLVertexBuffer(size);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI");
            }
        }

        public static VertexBuffer Create(float[] vertices, uint size)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None:    throw new ArgumentException("RendererAPI::None is currently not supported!"); 
                case RendererAPI.API.OpenGL:  return new OpenGLVertexBuffer(vertices, size);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI");
            }
        }
    }
    public abstract class IndexBuffer
    {
        public abstract  void Bind();

        public abstract  void Unbind();

        public abstract  uint GetCount();

        public static IndexBuffer Create(uint[] indices, uint size)
        {
            switch (Renderer.GetAPI())
            {
                case RendererAPI.API.None:    throw new ArgumentException("RendererAPI::None is currently not supported!"); 
                case RendererAPI.API.OpenGL:  return new OpenGLIndexBuffer(indices, size);
                default:
                    throw new ArgumentOutOfRangeException("Unknown RendererAPI");
            }
        }
    }
}