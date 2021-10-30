using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class Divider : RenderableComponent
    {
        public const float DefaultWidth = 10;
        public const float DefaultHeight = 10;
        private static readonly ColorRgba ActivatedColor = new(1, 1, 1, 1);

        private bool _visible;

        private readonly VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;

        public Divider()
        {
            Logger.Construct(nameof(Divider));

            _vertexArray = Application.Instance.Cache.VertexArray.Get("Positive Unit");

            _shaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("FlatColor_MVP_Uniform");

            Application.Instance.Input.CursorMoved += InputOnCursorMoved;
        }

        private void InputOnCursorMoved(float arg1, float arg2)
        {
            _visible = Maths.PointInRect(
                x: X,
                y: Y,
                width: PixelWidth,
                height: PixelHeight,
                px: Application.Instance.Input.GetCursorX(),
                py: Application.Instance.Input.GetCursorY()
            );
        }

        protected override void OnResized()
        {
            var scale = ScveMatrix4X4.CreateScale(PixelWidth, PixelHeight);
            ModelMatrix.MakeIdentity().Multiply(scale).Multiply(ScveMatrix4X4.CreateTranslation(X, Y, 0f));
            // Logger.Warn($"Divider set to ({X}, {Y}, {PixelWidth}, {PixelHeight})");
        }

        public override void AddChild(Component component)
        {
            throw new ScveException("Divider is not supposed to have any children");
        }

        public override void Render(IRenderer renderer)
        {
            if (!_visible)
            {
                return;
            }

            // renderer.RenderSolid(_vertexArray);
        }
    }
}