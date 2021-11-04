using SCVE.UI.Visitors;

namespace SCVE.UI.Containers
{
    /// <summary>
    /// Flex Cell occupies all space, provided by Flex Component, based on Flex property
    /// </summary>
    public class FlexCell : ContainerComponent
    {
        public float Flex { get; set; }

        public override void AcceptVisitor(IComponentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}