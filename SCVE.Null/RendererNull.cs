using SCVE.Core.Services;
using SCVE.Core.Utilities;

namespace SCVE.Null
{
    public class RendererNull : IRenderer
    {
        public void OnInit()
        {
            // Profiler.Invokations.Method();
            // Utils.PrintCurrentMethod();
        }

        public void OnTerminate()
        {
            // Profiler.Invokations.Method();
            // Utils.PrintCurrentMethod();
        }
    }
}