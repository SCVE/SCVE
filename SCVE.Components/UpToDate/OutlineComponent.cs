using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.UI;

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
            // Application.Instance.Input.CursorMoved += InputOnCursorMoved;
            // Application.Instance.Input.CursorLeave += InputOnCursorLeave;
        }

        // private void InputOnCursorLeave()
        // {
        //     _visible = false;
        // }

        // private void InputOnCursorMoved(float arg1, float arg2)
        // {
        //     _visible = Maths.PointInRect(X, Y, PixelWidth, PixelHeight, arg1, arg2);
        // }

        public override void Render(IRenderer renderer, float x, float y)
        {
            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer, x, y);
            }

            if (_visible)
            {
                // top
                renderer.RenderLine(x, y + _width / 2, y + ContentWidth, y + _width / 2, DefaultColor, _width);
                // right
                renderer.RenderLine(x + ContentWidth - _width / 2, y, x + ContentWidth - _width / 2, y + ContentHeight, DefaultColor, _width);
                // bottom
                renderer.RenderLine(x, y + ContentHeight - _width / 2, x + ContentWidth, y + ContentHeight - _width / 2, DefaultColor, _width);
                // left
                renderer.RenderLine(x + _width / 2, y, x + _width / 2, y + ContentHeight, DefaultColor, _width);

                // renderer.RenderWireframe(_vertexArray);
            }
        }
    }
}