using SCVE.Core.Rendering;
using SCVE.Core.Services;

namespace SCVE.Core.Entities
{
    public interface IRenderable
    {
        void Render(IRenderer renderer);
    }
}