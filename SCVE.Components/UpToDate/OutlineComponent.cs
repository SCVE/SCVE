using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.UI;
using SCVE.Core.Utilities;

namespace SCVE.Components.UpToDate
{
    public class OutlineComponent : RenderableComponent
    {
        private static readonly ColorRgba DefaultColor = new(1, 1, 1, 1);

        private float _width;

        private bool _visible;

        public OutlineComponent(float width = 4f)
        {
            _width = width;
            Application.Instance.Input.CursorMoved += InputOnCursorMoved;
            Application.Instance.Input.CursorLeave += InputOnCursorLeave;
        }

        private void InputOnCursorLeave()
        {
            _visible = false;
        }

        private void InputOnCursorMoved(float arg1, float arg2)
        {
            _visible = Maths.PointInRect(X, Y, PixelWidth, PixelHeight, arg1, arg2);
        }

        protected override void OnResized()
        {
            base.OnResized();
            Children[0].SetPositionAndSize(X, Y, PixelWidth, PixelHeight);
        }

        public override void Render(IRenderer renderer)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }

            if (_visible)
            {
                // top
                renderer.RenderLine(X, Y + _width / 2, X + PixelWidth, Y + _width / 2, DefaultColor, _width);
                // right
                renderer.RenderLine(X + PixelWidth - _width / 2, Y, X + PixelWidth - _width / 2, Y + PixelHeight, DefaultColor, _width);
                // bottom
                renderer.RenderLine(X, Y + PixelHeight - _width / 2, X + PixelWidth, Y + PixelHeight - _width / 2, DefaultColor, _width);
                // left
                renderer.RenderLine(X + _width / 2, Y, X + _width / 2, Y + PixelHeight, DefaultColor, _width);

                // renderer.RenderWireframe(_vertexArray);
            }
        }
    }
}