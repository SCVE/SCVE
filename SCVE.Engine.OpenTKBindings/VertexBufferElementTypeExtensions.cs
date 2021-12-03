using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Engine.Core.Rendering;

namespace SCVE.Engine.OpenTKBindings
{
    public static class VertexBufferElementTypeExtensions
    {
        public static VertexAttribPointerType ToGLType(this VertexBufferElementType type)
        {
            switch (type)
            {
                case VertexBufferElementType.Float:
                case VertexBufferElementType.Float2:
                case VertexBufferElementType.Float3:
                case VertexBufferElementType.Float4:
                    return VertexAttribPointerType.Float;
                case VertexBufferElementType.Int:
                case VertexBufferElementType.Int2:
                case VertexBufferElementType.Int3:
                case VertexBufferElementType.Int4:
                    return VertexAttribPointerType.Int;
                case VertexBufferElementType.Mat3:
                case VertexBufferElementType.Mat4:
                    return VertexAttribPointerType.Float;
                case VertexBufferElementType.Bool:
                    return VertexAttribPointerType.Byte;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}