using System;

namespace SCVE.Core.Rendering
{
    public abstract class Shader : IDisposable
    {
        public int Id;

        public abstract void Compile();

        public abstract void Dispose();
    }
}