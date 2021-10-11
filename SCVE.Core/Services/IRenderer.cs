using SCVE.Core.Entities;
using SCVE.Core.Lifecycle;

namespace SCVE.Core.Services
{
    public interface IRenderer : IDeferedInitable, ITerminatable
    {
        void Clear();

        void SetClearColor(Color color);
    }
}