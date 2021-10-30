using SCVE.Core.Primitives;
using SCVE.Core.Rendering;

namespace SCVE.Core.UI
{
    public abstract class RenderableComponent : Component
    {
        private ScveMatrix4X4 ScaleMatrix = ScveMatrix4X4.Identity;
        protected ScveMatrix4X4 TranslationMatrix = ScveMatrix4X4.Identity;
        public ScveMatrix4X4 ModelMatrix = ScveMatrix4X4.Identity;

        public RenderableComponent()
        {
        }

        public RenderableComponent(ComponentStyle style) : base(style)
        {
        }

        // protected override void OnResized()
        // {
        //     var scale = ScaleMatrix.MakeScale(PixelWidth, PixelHeight);
        //     var translation = TranslationMatrix.MakeTranslation(X, Y);
        //     ModelMatrix.MakeIdentity().Multiply(scale).Multiply(translation);
        // }
    }
}