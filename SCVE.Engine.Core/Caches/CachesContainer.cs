﻿namespace SCVE.Engine.Core.Caches
{
    public class CachesContainer
    {
        public ShaderProgramCache ShaderProgram { get; set; } = new();

        public VertexArrayCache VertexArray { get; set; } = new();
    }
}