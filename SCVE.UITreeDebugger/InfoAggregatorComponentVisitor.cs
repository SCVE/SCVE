using SCVE.UI;
using SCVE.UI.UpToDate;
using SCVE.UI.Visitors;

namespace SCVE.UITreeDebugger
{
    public class InfoAggregatorComponentVisitor : IComponentVisitor
    {
        private const int MaxIndentLevel = 20;
        private const char IndentChar = ' ';

        private readonly FlexComponent _flexComponent;

        private int _currentIndentLevel = 0;

        public InfoAggregatorComponentVisitor(FlexComponent flexComponent)
        {
            _flexComponent = flexComponent;
        }

        private void AddText(string text)
        {
            var cellInfo = new FlexCell()
            {
                Flex = 1
            };
            var cellSpace = new FlexCell()
            {
                Flex = MaxIndentLevel - 1
            };

            string indent = new string(IndentChar, _currentIndentLevel * 2);
            var textComponent = new TextComponent("arial.ttf", 14, indent + text, TextAlignment.Left);
            textComponent.SetStyle(ComponentStyle.Default);
            cellInfo.AddChild(textComponent);
            var flex = new FlexComponent();
            flex.AddChild(cellSpace);
            flex.AddChild(cellInfo);
            _flexComponent.AddChild(cellInfo);
        }

        public void Accept(EngineRunnableUI component)
        {
            string info = $"{nameof(EngineRunnableUI)}";
            AddText(info);

            _currentIndentLevel++;
            component.Component.AcceptVisitor(this);
            _currentIndentLevel--;
        }

        public void Accept(AlignComponent component)
        {
            string info = $"{nameof(AlignComponent)}";
            AddText(info);

            _currentIndentLevel++;
            component.Component.AcceptVisitor(this);
            _currentIndentLevel--;
        }

        public void Accept(BoxComponent component)
        {
            string info = $"{nameof(BoxComponent)}";
            AddText(info);

            _currentIndentLevel++;
            component.Component.AcceptVisitor(this);
            _currentIndentLevel--;
        }

        public void Accept(ClipComponent component)
        {
            string info = $"{nameof(ClipComponent)}";
            AddText(info);

            _currentIndentLevel++;
            component.Component.AcceptVisitor(this);
            _currentIndentLevel--;
        }

        public void Accept(ColorRectComponent component)
        {
            string info = $"{nameof(ColorRectComponent)}";
            AddText(info);
        }

        public void Accept(FlexCell component)
        {
            string info = $"{nameof(FlexCell)}";
            AddText(info);

            _currentIndentLevel++;
            component.Component.AcceptVisitor(this);
            _currentIndentLevel--;
        }

        public void Accept(FlexComponent component)
        {
            string info = $"{nameof(FlexComponent)}";
            AddText(info);

            _currentIndentLevel++;
            for (var i = 0; i < component.Children.Count; i++)
            {
                component.Children[i].AcceptVisitor(this);
            }

            _currentIndentLevel--;
        }

        public void Accept(FpsComponent component)
        {
            string info = $"{nameof(FpsComponent)}";
            AddText(info);
        }

        public void Accept(StackComponent component)
        {
            string info = $"{nameof(StackComponent)}";
            AddText(info);

            _currentIndentLevel++;
            for (var i = 0; i < component.Children.Count; i++)
            {
                component.Children[i].AcceptVisitor(this);
            }

            _currentIndentLevel--;
        }

        public void Accept(TextComponent component)
        {
            string info = $"{nameof(TextComponent)}";
            AddText(info);
        }

        public void Accept(PaddingComponent component)
        {
            string info = $"{nameof(PaddingComponent)}";
            AddText(info);
            
            _currentIndentLevel++;
            component.Component.AcceptVisitor(this);
            _currentIndentLevel--;
        }

        public void Accept(GlueComponent component)
        {
            string info = $"{nameof(GlueComponent)}";
            AddText(info);

            _currentIndentLevel++;
            for (var i = 0; i < component.Children.Count; i++)
            {
                component.Children[i].AcceptVisitor(this);
            }

            _currentIndentLevel--;
        }
    }
}