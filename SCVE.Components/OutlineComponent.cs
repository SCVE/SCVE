using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class OutlineComponent : RenderableComponent
    {
        private static readonly ColorRgba DefaultColor = new(1, 1, 1, 1);
        private readonly ColorRgba _colorRgba;
        private readonly VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;

        private bool _visible;
        
        public OutlineComponent()
        {
            _colorRgba = DefaultColor;
            
            _vertexArray = Application.Instance.Cache.VertexArray.Get("Positive Unit");

            _shaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("FlatColor_MVP_Uniform");
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
                _shaderProgram.SetVector4("u_Color", _colorRgba.R, _colorRgba.G, _colorRgba.B, _colorRgba.A);
            
                _shaderProgram.SetMatrix4("u_Model",
                    ModelMatrix
                );
                _shaderProgram.SetMatrix4("u_View",
                    Application.Instance.ViewProjectionAccessor.ViewMatrix
                );
                _shaderProgram.SetMatrix4("u_Proj",
                    Application.Instance.ViewProjectionAccessor.ProjectionMatrix
                );
                _shaderProgram.Bind();

                renderer.RenderWireframe(_vertexArray);
            }
        }
    }
}