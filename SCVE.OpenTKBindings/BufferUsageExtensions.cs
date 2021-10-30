using System;
using OpenTK.Graphics.OpenGL;
using SCVE.Core.Rendering;

namespace SCVE.OpenTKBindings
{
    public static class BufferUsageExtensions
    {
        public static BufferUsageHint ToOpenGlUsage(this BufferUsage usage)
        {
            return usage switch
            {
                BufferUsage.Static => BufferUsageHint.StaticDraw,
                BufferUsage.Dynamic => BufferUsageHint.DynamicDraw,
                BufferUsage.Stream => BufferUsageHint.StreamDraw,
                _ => throw new ArgumentOutOfRangeException(nameof(usage), usage, null)
            };
        }
    }
}