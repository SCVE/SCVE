using System.Collections.Generic;
using SCVE.Core.Entities;

namespace SCVE.Core
{
    public interface IComponent : IRenderable
    {
        List<IComponent> Children { get; }
    }
}