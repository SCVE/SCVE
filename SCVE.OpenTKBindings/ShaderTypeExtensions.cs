using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public static class ShaderTypeExtensions
    {
        public static ShaderType ToGLShaderType(this ScveShaderType type)
        {
            return type switch
            {
                ScveShaderType.Vertex => ShaderType.VertexShader,
                ScveShaderType.Fragment => ShaderType.FragmentShader,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}