using SCVE.Core.Entities;

namespace SCVE.Core.Rendering
{
    public abstract class RenderingContext
    {
        public ScveWindow Window { get; private set; }

        public RenderingContext(ScveWindow window)
        {
            Window = window;
        }

        public abstract void Init();

        public abstract void SwapBuffers();
    }
}