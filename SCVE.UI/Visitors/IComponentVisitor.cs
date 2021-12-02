﻿using SCVE.UI.Containers;
using SCVE.UI.Elements;
using SCVE.UI.Groups;
using SCVE.UI.Primitive;

namespace SCVE.UI.Visitors
{
    public interface IComponentVisitor
    {
        public void Visit(Component component)
        {
            component.AcceptVisitor(this);
        }

        void Accept(EngineRunnableUI component);
        void Accept(AlignComponent component);
        void Accept(BoxComponent component);
        void Accept(ClipComponent component);
        void Accept(ColorRectComponent component);
        void Accept(FlexCell component);
        void Accept(FlexComponent component);
        void Accept(FpsComponent component);
        void Accept(StackComponent component);
        void Accept(TextComponent component);
        void Accept(PaddingComponent component);
        void Accept(GlueComponent component);
        void Accept(TemplateComponent component);
        void Accept(ButtonContainerComponent component);
        void Accept(ButtonComponent component);
    }
}