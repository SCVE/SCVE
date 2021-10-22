using SCVE.Core.Entities;

namespace SCVE.Core.Rendering
{
    /// <summary>
    /// Base Context for a window, as of now responsible for Swapping Buffers
    /// </summary>
    public abstract class Context
    {
        public ScveWindow Window { get; private set; }

        public Context(ScveWindow window)
        {
            Window = window;
        }

        public abstract void Init();

        public abstract void SwapBuffers();
    }
}