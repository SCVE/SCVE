using SCVE.Core.Input;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class GlfwEngineInput : EngineInput
    {
        public GlfwEngineInput()
        {
            Logger.Construct(nameof(GlfwEngineInput));
        }
    }
}