using SCVE.Core.Entities;
using SCVE.Core.Services;
using SCVE.Core.Utilities;

namespace SCVE.Null
{
    public class RendererNull : IRenderer
    {
        public void OnDeferInit()
        {
            Logger.Trace("RendererNull.OnInit");
        }

        public void OnTerminate()
        {
            Logger.Trace("RendererNull.OnTerminate");
        }

        public void Clear()
        {
            Logger.Trace("RendererNull.Clear");
        }

        public void SetClearColor(Color color)
        {
            Logger.Trace("RendererNull.SetClearColor");
        }
    }
}