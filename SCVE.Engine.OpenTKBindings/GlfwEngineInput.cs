using SCVE.Engine.Core.Input;
using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.OpenTKBindings
{
    public class GlfwEngineInput : EngineInput
    {
        public GlfwEngineInput()
        {
            Logger.Construct(nameof(GlfwEngineInput));
        }
    }
}