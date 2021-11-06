using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;
using SCVE.UI.Primitive;

namespace SCVE.UI.Elements
{
    public class ButtonComponent : Component
    {
        private ColorRectComponent _backgroundRect;
        private TextComponent _textComponent;

        public ColorRgba BackgroundColor
        {
            get => _backgroundRect.Style.PrimaryColor.Value;
            set => _backgroundRect.Style.PrimaryColor.Value = value;
        }

        public string Text
        {
            get => _textComponent.GetText();
            set
            {
                if (_textComponent.GetText() != value)
                {
                    _textComponent.SetText(value);
                }
            }
        }

        public override void Init()
        {
            base.Init();
            _backgroundRect.Init();
            _textComponent.Init();
        }

        public override void OnSetStyle()
        {
            base.OnSetStyle();
            _backgroundRect.SetStyle(Style);
            _textComponent.SetStyle(Style);
        }

        public ButtonComponent()
        {
            _backgroundRect = new ColorRectComponent();
            _textComponent  = new TextComponent();
        }

        public override void Measure(float availableWidth, float availableHeight)
        {
            DesiredWidth  = Style.Width.Flatten(availableWidth);
            DesiredHeight = Style.Height.Flatten(availableHeight);
            _backgroundRect.Measure(availableWidth, availableHeight);
            _textComponent.Measure(availableWidth, availableHeight);
        }

        public override void Arrange(float x, float y, float availableWidth, float availableHeight)
        {
            X      = x;
            Y      = y;
            Width  = DesiredWidth;
            Height = DesiredHeight;
            _backgroundRect.Arrange(x, y, Width, Height);
            _textComponent.Arrange(x, y, Width, Height);
        }

        public override Component PickComponentByPosition(float x, float y)
        {
            if (Maths.PointInRect(X, Y, Width, Height, x, y))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        protected override void SubtreeUpdated()
        {
            Measure(Width, Height);
            Arrange(X, Y, Width, Height);
        }

        public override void BubbleEvent(string name)
        {
            base.BubbleEvent(name);
            Logger.Warn($"Button received: {name}");
        }

        public override void RenderSelf(IRenderer renderer)
        {
            _backgroundRect.RenderSelf(renderer);
            _textComponent.RenderSelf(renderer);
        }
    }
}