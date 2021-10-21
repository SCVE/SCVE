using SCVE.Core.Input;
using SCVE.Core.Utilities;

namespace SCVE.OpenTKBindings
{
    public class GlfwInput : InputBase
    {
        public GlfwInput()
        {
            Logger.Construct(nameof(GlfwInput));
        }
    }
}