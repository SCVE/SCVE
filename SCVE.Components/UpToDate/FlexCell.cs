using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    /// <summary>
    /// Flex Cell occupies all space, provided by Flex Component, based on Flex property
    /// </summary>
    public class FlexCell : ContainerComponent
    {
        public float Flex { get; set; }

        public override void PrintComponentTree(int indent)
        {
            Logger.WarnIndent($"{nameof(FlexCell)} {X}:{Y}:{Width}:{Height}", indent);
            Component.PrintComponentTree(indent + 1);
        }
    }
}