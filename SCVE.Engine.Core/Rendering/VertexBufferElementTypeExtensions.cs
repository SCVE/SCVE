using System;

namespace SCVE.Engine.Core.Rendering
{
    public static class VertexBufferElementTypeExtensions
    {
        /// <summary>
        /// Returns an amount of components in a single element
        /// </summary>
        public static int ComponentCount(this VertexBufferElementType type)
        {
            switch (type)
            {
                case VertexBufferElementType.Float:
                    return 1;
                case VertexBufferElementType.Float2:
                    return 2;
                case VertexBufferElementType.Float3:
                    return 3;
                case VertexBufferElementType.Float4:
                    return 4;
                case VertexBufferElementType.Int:
                    return 1;
                case VertexBufferElementType.Int2:
                    return 2;
                case VertexBufferElementType.Int3:
                    return 3;
                case VertexBufferElementType.Int4:
                    return 4;
                case VertexBufferElementType.Mat3:
                    return 3; // 3 float3
                case VertexBufferElementType.Mat4:
                    return 4; // 4 float4
                case VertexBufferElementType.Bool:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        /// Returns an amount of bytes required for a single element
        /// </summary>
        public static int SizeInBytes(this VertexBufferElementType type)
        {
            switch (type)
            {
                case VertexBufferElementType.Float:
                    return sizeof(float);
                case VertexBufferElementType.Float2:
                    return sizeof(float) * 2;
                case VertexBufferElementType.Float3:
                    return sizeof(float) * 3;
                case VertexBufferElementType.Float4:
                    return sizeof(float) * 4;
                case VertexBufferElementType.Int:
                    return sizeof(int);
                case VertexBufferElementType.Int2:
                    return sizeof(int) * 2;
                case VertexBufferElementType.Int3:
                    return sizeof(int) * 3;
                case VertexBufferElementType.Int4:
                    return sizeof(int) * 4;
                case VertexBufferElementType.Mat3:
                    return sizeof(float) * 3 * 3;
                case VertexBufferElementType.Mat4:
                    return sizeof(float) * 4 * 4;
                case VertexBufferElementType.Bool:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}